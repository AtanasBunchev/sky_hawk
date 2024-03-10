using Docker.DotNet;
using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Contexts;
using SkyHawk.Data.Entities;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

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
        return new(await _context.Users.SingleOrDefaultAsync(x => x.Username == request.Name));
    }

    public async Task<GetUserResponse> GetUserByIdAsync(GetByIdRequest request)
    {
        return new(await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id));
    }


    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var maxUsernameLength = typeof(User).GetProperty("Username")
                .GetCustomAttribute<MaxLengthAttribute>().Length;
        if(request.Username.Length > maxUsernameLength) {
            return new(BusinessStatusCodeEnum.InvalidInput,
                    $"Username must be less than {maxUsernameLength} symbols long!");
        }

        var minUsernameLength = typeof(User).GetProperty("Username")
                .GetCustomAttribute<MinLengthAttribute>().Length;
        if(request.Username.Length < minUsernameLength) {
            return new(BusinessStatusCodeEnum.InvalidInput,
                    $"Username must be at least {minUsernameLength} symbols long!");
        }

        var maxPasswordLength = typeof(User).GetProperty("Password")
                .GetCustomAttribute<MaxLengthAttribute>().Length;
        if(request.Password.Length > maxPasswordLength) {
            return new(BusinessStatusCodeEnum.InvalidInput,
                    $"Password must be less than {maxPasswordLength} symbols long!");
        }

        var minPasswordLength = typeof(User).GetProperty("Password")
                .GetCustomAttribute<MinLengthAttribute>().Length;
        if(request.Password.Length < minPasswordLength) {
            return new(BusinessStatusCodeEnum.InvalidInput,
                    $"Password must be at least {minPasswordLength} symbols long!");
        }


        var existenceCheck = await GetUserByNameAsync(new(request.Username));
        if(existenceCheck.StatusCode != BusinessStatusCodeEnum.NotFound) {
            return new(BusinessStatusCodeEnum.EntryAlreadyExists, "This username is already used!");
        }

        User user = new () {
            Username = request.Username,
            Password = request.Password
        };
        _context.Add(user);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "User created successfully.");
    }


    public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented, "Not implemented");
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
