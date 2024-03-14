using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestListUsers()
    {
        var emptyResponse = await _service.ListUsersAsync(new());
        Assert.Empty(emptyResponse.Users);

        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.ListUsersAsync(new());

        Assert.NotEmpty(response.Users);
        Assert.Single(response.Users);
        foreach(var returnedUser in response.Users) {
            Assert.Equal(user.Id, returnedUser.Id);
            Assert.Equal(user.Username, returnedUser.Username);
            Assert.Equal(user.CreateTime, returnedUser.CreateTime);
            Assert.Equal(user.LastLogin, returnedUser.LastLogin);
            Assert.Equal(user.MaxServers, returnedUser.MaxServers);
            Assert.Equal(user.CanMakeSnapshots, returnedUser.CanMakeSnapshots);
        }
    }

    // TODO list paged requests
}
