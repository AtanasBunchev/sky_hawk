using Microsoft.EntityFrameworkCore;
using SkyHawk.Data.Entities;
using SkyHawk.ApplicationServices.Messaging;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests : IDisposable
{
    /*
    [Fact]
    public async void UpdateServerPort_Succeeds()
    {
        var server = GetValidServerEntity();
        server.Port = 2000;
        _context.Servers.Add(server);
        _context.SaveChanges();

        var response = await _service.UpdateServerPortAsync(new(_user, server.Id, 5000));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        
        server = await _context.Servers.SingleOrDefaultAsync(x => x.Id == server.Id);
        Assert.Equal(5000, server.Port);

        // TODO finish this up
    }

    public async void UpdateServerPort_NotFound_Fails()
    {
        var response = await _service.UpdateServerPortAsync(new(_user, 10, 5000));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Empty(_context.Servers);
    }
    */
}
