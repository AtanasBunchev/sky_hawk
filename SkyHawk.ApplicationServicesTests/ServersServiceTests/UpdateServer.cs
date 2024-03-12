using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests
{
    [Fact]
    public async void TestUpdateServer_ValidData_Succeeds()
    {
        var server = GetValidServerEntity();
        _context.Servers.Add(server);
        _context.SaveChanges();

        for(int i = 0; i < (1 << 6) - 1; i++) {
            UpdateServerRequest request = new (_user, server.Id);

            if((i & 1) == 1) {
                request.Port = 1000 + (i * i + 5) % 8080;
            }
            if((i & 2) == 2) {
                request.Name = $"Test {i}";
            }
            if((i & 4) == 4) {
                request.Description = $"Test {i} runs now";
            }
            if((i & 8) == 8) {
                request.Image = new MemoryStream(new byte[4] {0, (byte)i, 0, (byte)i});
            }
            if((i & 16) == 16) {
                request.AutoStart = new (i % 24, i % 60, 5 + i % 55);
            }
            if((i & 32) == 32) {
                request.AutoStop = new (i % 24, i % 60, 5 + i % 55);
            }

            var response = await _service.UpdateServerAsync(request);

            Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
            Assert.Equal(request.Port, server.Port);
            Assert.Equal(request.Name, server.Name);
            Assert.Equal(request.Description, server.Description);
            Assert.Equal(request.AutoStart, server.AutoStart);
            Assert.Equal(request.AutoStop, server.AutoStop);

            if(request.Image != null)
                for(int j = 0; j < server.Image.Length; j++)
                    Assert.Equal(request.Image.ReadByte(), server.Image[j]);
        }
    }

    [Fact]
    public async void TestUpdateServer_InvalidName_Fails()
    {
        var server = GetValidServerEntity();
        _context.Servers.Add(server);
        _context.SaveChanges();

        var invalidName = new String('u', 512); // Too Long
        var originalName = server.Name;

        UpdateServerRequest request = new (_user, server.Id) {
            Name = invalidName
        };
        var response = await _service.UpdateServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
        Assert.Equal(originalName, server.Name);
    }

    [Fact]
    public async void TestUpdateServer_EqualAutoStartAndStop_ClearsAutoStartAndStop()
    {
        var server = GetValidServerEntity();
        server.AutoStart = new(18, 0, 0);
        server.AutoStop = new(22, 0, 0);
        _context.Servers.Add(server);
        _context.SaveChanges();

        UpdateServerRequest request = new (_user, server.Id) {
            AutoStart = new(0, 0, 5),
            AutoStop = new(0, 0, 5),
        };
        var response = await _service.UpdateServerAsync(request);

        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Null(server.AutoStart);
        Assert.Null(server.AutoStop);
    }

    [Fact]
    public async void TestUpdateServer_NotExists_Fails()
    {
        UpdateServerRequest request = new (_user, 10);
        var response = await _service.UpdateServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
