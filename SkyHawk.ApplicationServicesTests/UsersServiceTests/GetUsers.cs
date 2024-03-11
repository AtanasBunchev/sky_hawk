using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestGetUsers()
    {
        var emptyResponse = await _service.GetUsersAsync(new());
        Assert.Empty(emptyResponse.Users);

        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUsersAsync(new());

        Assert.NotEmpty(response.Users);
        Assert.Single(response.Users);
        foreach(var returnedUser in response.Users)
            Assert.Equal(user, returnedUser);
    }
}
