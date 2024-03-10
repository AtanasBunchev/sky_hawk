using SkyHawk.Data.Contexts;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;

namespace SkyHawk.ApplicationServices.Implementation;

public class UsersService : IUsersService
{
    SkyHawkDbContext _context;

    public UsersService(SkyHawkDbContext context)
    {
        _context = context;
    }


    public async Task<GetUsersResponse> GetUsersAsync(GetUsersRequest request)
    {
        return new();
    }


    public async Task<GetUserResponse> GetUserByNameAsync(GetByNameRequest request)
    {
        return new();
    }

    public async Task<GetUserResponse> GetUserByIdAsync(GetByIdRequest request)
    {
        return new();
    }


    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        return new();
    }


    public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        return new();
    }


    public async Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request)
    {
        return new();
    }


    public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request)
    {
        return new();
    }
}
