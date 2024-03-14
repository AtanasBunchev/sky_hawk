using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests : IDisposable
{
    [Fact]
    public async void TestListServers()
    {
        var emptyResponse = await _service.ListServersAsync(new(_user));
        Assert.Empty(emptyResponse.Servers);

        var server = GetValidServerEntity();
        _context.Servers.Add(server);
        _context.SaveChanges();

        var response = await _service.ListServersAsync(new(_user));

        Assert.NotEmpty(response.Servers);
        Assert.Single(response.Servers);
        foreach(var returnedServer in response.Servers)
            Assert.Equal(server, returnedServer);
    }

    // TODO list paged requests
}
