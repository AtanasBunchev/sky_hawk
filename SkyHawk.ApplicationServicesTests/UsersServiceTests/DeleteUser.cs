using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestDeleteUser()
    {
        var user = this.GetValidUserEntity();
        _context.Add(user);
        _context.SaveChanges();
        int id = user.Id;

        var response = await _service.DeleteUserAsync(new(id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Empty(_context.Users.ToList());

        response = await _service.DeleteUserAsync(new(id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
