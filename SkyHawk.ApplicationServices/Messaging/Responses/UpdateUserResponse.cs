namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class UpdateUserResponse : ResponseBase
{
    public UpdateUserResponse(BusinessStatusCodeEnum statusCode, string messageText)
        : base(statusCode, messageText)
    {

    }
};
