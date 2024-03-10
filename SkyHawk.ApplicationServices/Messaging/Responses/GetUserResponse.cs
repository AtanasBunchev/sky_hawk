using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetUserResponse
{
    public User? User { get; set; }

    public GetUserResponse(User? user)
    {
        User = user;
    }
};
