using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestGenerateJwtToken_Succeeds()
    {
        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GenerateJwtTokenAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.BearerToken);

        TestJwtToken(user, response.BearerToken);
    }

    [Fact]
    public async void TestGenerateJwtToken_NotExists_Fails()
    {
        var user = GetValidUserEntity();

        var response = await _service.GenerateJwtTokenAsync(new(user.Id));
        Assert.NotEqual(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Null(response.BearerToken);
    }
}
