using Docker.DotNet;
using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Contexts;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Implementation;

public class UsersService : IUsersService
{
    SkyHawkDbContext _context;
    IDockerClient _docker;

    public UsersService(SkyHawkDbContext context, IDockerClient docker)
    {
        _context = context;
        _docker = docker;
    }


    public async Task<GetUsersResponse> GetUsersAsync(GetUsersRequest request)
    {
        return new(await _context.Users.ToListAsync());
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
