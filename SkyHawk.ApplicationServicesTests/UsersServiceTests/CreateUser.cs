using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestCreateUser_ValidData_Succeeds()
    {
        var template = this.GetValidUserEntity();

        // Test user creation
        var response = await _service.CreateUserAsync(new(template.Username, template.Password));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm user created
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == template.Username);
        Assert.NotNull(user);

        // Compare data
        Assert.Equal(template.Password, user.Password);
        // either check hash or authenticate once password is hashed
    }

    [Fact]
    public async void TestCreateUser_InvalidUsername_Fails()
    {
        var template = this.GetValidUserEntity();

        // Test too short username
        var response = await _service.CreateUserAsync(new("u", template.Password));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Test too long username
        response = await _service.CreateUserAsync(new(new String('u', 200), template.Password));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm user not created
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == "u");
        Assert.Null(user);
    }

    [Fact]
    public async void TestCreateUser_InvalidPassword_Fails()
    {
        var template = this.GetValidUserEntity();

        // Test too short password
        var response = await _service.CreateUserAsync(new(template.Username, "p"));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm user not created
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == template.Password);
        Assert.Null(user);
    }

    [Fact]
    public async void TestCreateUser_AlreadyExists_Fails()
    {
        var user = this.GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        // Confirm error on existing user
        var response = await _service.CreateUserAsync(new(user.Username, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.EntryAlreadyExists, response.StatusCode);

        // Confirm user not created
        var collection = await _context.Users.ToListAsync();
        Assert.Single(collection);
        Assert.Equal(user, collection[0]);
    }
}
