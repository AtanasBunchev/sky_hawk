using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;

namespace SkyHawk.ApplicationServices.Interfaces;

public interface IUsersService
{
    // List users
    public Task<ListUsersResponse> ListUsersAsync(ListUsersRequest request);

    // Get user
    public Task<GetUserResponse> GetUserByNameAsync(GetUserByNameRequest request);
    public Task<GetUserResponse> GetUserByIdAsync(GetUserByIdRequest request);

    // Create user
    public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);

    // Modify user
    public Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);

    // Delete user
    public Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request);


    // Authentication
    public Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request);

    public Task<GenerateJwtTokenResponse> GenerateJwtTokenAsync(GetUserByIdRequest request);
}
