using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestAuthenticateUser_Succeeds()
    {
        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.AuthenticateUserAsync(new(user.Username, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.BearerToken);

        TestJwtToken(user, response.BearerToken);
    }

    [Fact]
    public async void TestAuthenticateUser_WrongPassword_Fails()
    {
        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.AuthenticateUserAsync(new(user.Username, user.Password + "p"));
        Assert.Equal(BusinessStatusCodeEnum.AuthenticationFailed, response.StatusCode);
        Assert.Null(response.BearerToken);
    }

    [Fact]
    public async void TestAuthenticateUser_NotExists_Fails()
    {
        var user = GetValidUserEntity();

        var response = await _service.AuthenticateUserAsync(new(user.Username, user.Password));
        Assert.NotEqual(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Null(response.BearerToken);
    }
}
