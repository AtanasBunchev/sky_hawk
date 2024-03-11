namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class CreateUserResponse : ResponseBase
{
    public CreateUserResponse(BusinessStatusCodeEnum statusCode, string messageText)
        : base(statusCode, messageText)
    {

    }
};
