using SkyHawk.Data.Entities;
using SkyHawk.ApplicationServices.Messaging.ViewModels;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetUserResponse : ResponseBase
{
    public UserVM? User { get; set; }

    public GetUserResponse(UserVM? user)
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
