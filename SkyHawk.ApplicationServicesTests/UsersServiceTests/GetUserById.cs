using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestGetUserById()
    {
        var user = this.GetValidUserEntity();

        var nullResponse = await _service.GetUserByIdAsync(new(10));
        Assert.Null(nullResponse.User);

        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUserByIdAsync(new(user.Id));

        Assert.Equal(user, response.User);
    }
}
