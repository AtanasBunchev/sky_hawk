using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestGetUserById()
    {
        var user = GetValidUserEntity();

        var nullResponse = await _service.GetUserByIdAsync(new(10));
        Assert.Null(nullResponse.User);

        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUserByIdAsync(new(user.Id));

        Assert.Equal(user.Id, response.User.Id);
        Assert.Equal(user.Username, response.User.Username);
        Assert.Equal(user.CreateTime, response.User.CreateTime);
        Assert.Equal(user.MaxServers, response.User.MaxServers);
        Assert.Equal(user.MaxRunningServers, response.User.MaxRunningServers);
        Assert.Equal(user.CanMakeSnapshots, response.User.CanMakeSnapshots);
    }
}
