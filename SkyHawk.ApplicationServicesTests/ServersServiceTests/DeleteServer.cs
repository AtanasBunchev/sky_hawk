using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests
{
    [Fact]
    public async void TestDeleteServer()
    {
        var server = GetValidServerEntity();
        _context.Add(server);
        _context.SaveChanges();
        int id = server.Id;

        var response = await _service.DeleteServerAsync(new(_user, id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Empty(_context.Servers.ToList());

        response = await _service.DeleteServerAsync(new(_user, id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
