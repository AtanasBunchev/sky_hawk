using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestGetUserByName()
    {
        var user = this.GetValidUserEntity();

        var nullResponse = await _service.GetUserByNameAsync(new(user.Username));
        Assert.Null(nullResponse.User);

        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUserByNameAsync(new(user.Username));
        Assert.Equal(user, response.User);
    }
}
