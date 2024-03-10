using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestCreateUser_Correct()
    {
        var template = this.GetValidUserEntity();

        var response = await _service.CreateUserAsync(new(template.Username, template.Password));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == template.Username);
        Assert.NotNull(user);

        // either check hash or authenticate once password is hashed
        Assert.Equal(template.Password, user.Password);
    }

    [Fact]
    public async void TestCreateUser_InvalidUsername()
    {
        var template = this.GetValidUserEntity();

        var response = await _service.CreateUserAsync(new("", template.Password));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == "");
        Assert.Null(user);
    }

    [Fact]
    public async void TestCreateUser_AlreadyExists()
    {
        var user = this.GetValidUserEntity();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.CreateUserAsync(new(user.Username, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.EntryAlreadyExists, response.StatusCode);

        var collection = await _context.Users.ToListAsync();
        Assert.Single(collection);
        Assert.Equal(user, collection[0]);
    }
}
