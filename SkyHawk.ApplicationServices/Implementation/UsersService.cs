using Docker.DotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Contexts;
using SkyHawk.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

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


    public async Task<ListUsersResponse> ListUsersAsync(ListUsersRequest request)
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
                ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
        if(maxUsernameLength != null && request.Username.Length > maxUsernameLength) {
            return new(BusinessStatusCodeEnum.InvalidInput,
                    $"Username must be less than {maxUsernameLength} symbols long!");
        }

        var minUsernameLength = typeof(User).GetProperty("Username")
                ?.GetCustomAttribute<MinLengthAttribute>()?.Length;
        if(minUsernameLength != null && request.Username.Length < minUsernameLength) {
            return new(BusinessStatusCodeEnum.InvalidInput,
                    $"Username must be at least {minUsernameLength} symbols long!");
        }

        var maxPasswordLength = typeof(User).GetProperty("Password")
                ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
        if(maxPasswordLength != null && request.Password.Length > maxPasswordLength) {
            return new(BusinessStatusCodeEnum.InvalidInput,
                    $"Password must be less than {maxPasswordLength} symbols long!");
        }

        var minPasswordLength = typeof(User).GetProperty("Password")
                ?.GetCustomAttribute<MinLengthAttribute>()?.Length;
        if(minPasswordLength != null && request.Password.Length < minPasswordLength) {
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
        var user = (await GetUserByIdAsync(new(request.Id))).User;
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");


        if(request.Username != null) {
            var maxUsernameLength = typeof(User).GetProperty("Username")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxUsernameLength != null && request.Username.Length > maxUsernameLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Username must be less than {maxUsernameLength} symbols long!");
            }

            var minUsernameLength = typeof(User).GetProperty("Username")
                    ?.GetCustomAttribute<MinLengthAttribute>()?.Length;
            if(minUsernameLength != null && request.Username.Length < minUsernameLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Username must be at least {minUsernameLength} symbols long!");
            }

            user.Username = request.Username;
        }

        if(request.Password != null) {
            var maxPasswordLength = typeof(User).GetProperty("Password")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxPasswordLength != null && request.Password.Length > maxPasswordLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Password must be less than {maxPasswordLength} symbols long!");
            }

            var minPasswordLength = typeof(User).GetProperty("Password")
                    ?.GetCustomAttribute<MinLengthAttribute>()?.Length;
            if(minPasswordLength != null && request.Password.Length < minPasswordLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Password must be at least {minPasswordLength} symbols long!");
            }

            user.Password = request.Password;
        }

        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "User updated successfully.");
    }


    public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request)
    {
        var user = (await GetUserByIdAsync(new(request.Id))).User;
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");

        _context.Remove(user);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "User deleted successfully.");
    }


    public async Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request)
    {
        User? user = (await GetUserByNameAsync(new(request.Username))).User;
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");


        if(user.Password != request.Password)
            return new(BusinessStatusCodeEnum.AuthenticationFailed, "Wrong password!");

        string token = GenerateJwtTokenInternal(user);

        return new(token, "Authentication successfull.");
    }

    public async Task<GenerateJwtTokenResponse> GenerateJwtTokenAsync(GetByIdRequest request)
    {
        User? user = (await GetUserByIdAsync(new(request.Id))).User;
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");

        string token = GenerateJwtTokenInternal(user);

        return new(token, "JWT Token generated successfully.");
    }


    private string GenerateJwtTokenInternal(User user)
    {
        var claims = new[]
        {
            new Claim("LoggedUserId", user.Id.ToString())
        };

        var signingKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("785e26cdd9464e8b92c0cd41ae8df74c")
        );

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "SkyHawk", // issue
            "SkyHawk", // audience
            claims,
            expires: DateTime.UtcNow.AddMinutes(90),
            signingCredentials: signingCredentials
        );


        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }
}
