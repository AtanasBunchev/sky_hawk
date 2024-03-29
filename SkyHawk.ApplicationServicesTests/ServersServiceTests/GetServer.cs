using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests : IDisposable
{
    [Fact]
    public async void TestGetServerById()
    {
        var server = GetValidServerEntity();

        var nullResponse = await _service.GetServerByIdAsync(new(_user, 10));
        Assert.Null(nullResponse.Server);

        _context.Servers.Add(server);
        _context.SaveChanges();

        var response = await _service.GetServerByIdAsync(new(_user, server.Id));

        Assert.Equal(server, response.Server);
    }

    [Fact]
    public async void TestGetServerByName()
    {
        var server = GetValidServerEntity();

        var nullResponse = await _service.GetServerByNameAsync(new(_user, server.Name));
        Assert.Null(nullResponse.Server);

        _context.Servers.Add(server);
        _context.SaveChanges();

        var response = await _service.GetServerByNameAsync(new(_user, server.Name));
        Assert.Equal(server, response.Server);
    }
}
