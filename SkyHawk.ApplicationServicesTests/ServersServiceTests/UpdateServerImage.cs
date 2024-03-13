using Microsoft.EntityFrameworkCore;
using SkyHawk.Data.Entities;
using SkyHawk.ApplicationServices.Messaging;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests
{
    [Fact]
    public async void UpdateServerImage_Succeeds()
    {
        var server = GetValidServerEntity();
        server.Image = null;
        _context.Servers.Add(server);
        _context.SaveChanges();

        MemoryStream input = new (100);
        for(int i = 0; i < 100; i++)
            input.WriteByte((byte) ((i * 41 + 13) % 97));
        input.Position = 0;

        var response = await _service.UpdateServerImageAsync(new(_user, server.Id, input));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Equal(server, await _context.Servers.SingleOrDefaultAsync(x => x.Id == server.Id));
        Assert.NotNull(server.Image);
        Assert.Equal(100, server.Image.Length);
        for(int i = 0; i < 100; i++) {
            if((i * 41 + 13) % 97 != server.Image[i]) {
                Assert.Fail("Blob not stored correctly");
            }
        }
    }

    [Fact]
    public async void UpdateServerImage_NotFound_Fails()
    {
        MemoryStream input = new (100);
        for(int i = 0; i < 100; i++)
            input.WriteByte((byte) ((i * 41 + 13) % 97));

        var response = await _service.UpdateServerImageAsync(new(_user, 10, input));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Empty(_context.Servers);
    }
}
