using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetUserResponse : ResponseBase
{
    public User? User { get; set; }

    public GetUserResponse(User? user)
    {
        User = user;
        if (user == null) {
            StatusCode = BusinessStatusCodeEnum.NotFound;
        } else {
            StatusCode = BusinessStatusCodeEnum.Success;
        }
    }
};
