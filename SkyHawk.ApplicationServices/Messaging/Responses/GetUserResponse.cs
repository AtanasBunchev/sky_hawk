using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetUserResponse : ResponseBase
{
    public User? User { get; set; }

    public GetUserResponse(User? user)
        : base(BusinessStatusCodeEnum.None)
    {
        User = user;
        if (user == null) {
            StatusCode = BusinessStatusCodeEnum.NotFound;
            MessageText = "User not found!";
        } else {
            StatusCode = BusinessStatusCodeEnum.Success;
            MessageText = "User fetched successfully.";
        }
    }
};
