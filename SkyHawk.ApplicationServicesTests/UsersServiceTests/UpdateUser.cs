using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestUpdateUser_ValidData_Succeeds()
    {
        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        for(int i = 0; i < 4; i++) {
            var targetName = user.Username;
            var targetPass = user.Password;

            UpdateUserRequest request = new (user.Id);

            if((i & 1) == 1) {
                request.Username = targetName + "u";
                targetName += "u";
            }
            if((i & 2) == 2) {
                request.Password = targetPass + "p";
                targetPass += "p";
            }

            var response = await _service.UpdateUserAsync(request);

            Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
            Assert.Equal(targetName, user.Username);
            Assert.Equal(targetPass, user.Password);
        }
    }

    [Fact]
    public async void TestUpdateUser_InvalidUsername_Fails()
    {
        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var invalidName = "u"; // Too Short
        var originalName = user.Username;

        UpdateUserRequest request = new (user.Id) { Username = invalidName };
        var response = await _service.UpdateUserAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        Assert.Equal(originalName, user.Username);
    }

    [Fact]
    public async void TestUpdateUser_InvalidPassword_Fails()
    {
        var user = GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var invalidPass = "p"; // Too Short
        var originalPass = user.Password;

        UpdateUserRequest request = new (user.Id) { Password = invalidPass };
        var response = await _service.UpdateUserAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        Assert.Equal(originalPass, user.Password);
    }

    [Fact]
    public async void TestUpdateUser_NotExists_Fails()
    {
        UpdateUserRequest request = new (10);
        var response = await _service.UpdateUserAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
