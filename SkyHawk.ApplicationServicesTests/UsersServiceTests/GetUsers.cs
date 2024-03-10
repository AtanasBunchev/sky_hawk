using SkyHawk.ApplicationServices.Implementation;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestGetUsers()
    {
        var emptyResponse = await _service.GetUsersAsync(new());
        Assert.Empty(emptyResponse.Users);

        var user = this.GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUsersAsync(new());

        Assert.NotEmpty(response.Users);
        Assert.Equal(1, response.Users.Count);
        foreach(var returnedUser in response.Users)
            Assert.Equal(user, returnedUser);
    }
}
