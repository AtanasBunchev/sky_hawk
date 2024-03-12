using SkyHawk.Data.Entities;
using SkyHawk.Data.Server;
using SkyHawk.ApplicationServices.Messaging;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests
{
    [Fact]
    public async void TestCreateServerFromImage_CreatesServer()
    {
        ServerType type = ServerType.MinetestGame;
        int port = 30512;
        string name = "New server";
        string description = "Description";

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port) {
                Name = name,
                Description = description
            });
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Single(_context.Servers);
        var server = _context.Servers.ToList()[0];

        Assert.Equal(type, server.Type);
        Assert.Equal(port, server.Port);
        Assert.Equal(name, server.Name);
        Assert.Equal(description, server.Description);
    }

    [Fact]
    public async void TestCreateServerFromImage_CreatesContainer()
    {
        ServerType type = ServerType.MinetestGame;
        int port = 30512;

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void TestCreateServerFromImage_InvalidType_Fails()
    {
        ServerType type = ServerType.Unknown;
        int port = 30512;

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }

    [Fact]
    public async void TestCreateServerFromImage_TooLowPort_Fails()
    {
        ServerType type = ServerType.MinetestGame;
        int port = 1000;

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }

    [Fact]
    public async void TestCreateServerFromImage_InvalidName_Fails()
    {
        ServerType type = ServerType.MinetestGame;
        int port = 30512;
        string name = new String('n', 512); // too long

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port) { Name = name });
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }
}
