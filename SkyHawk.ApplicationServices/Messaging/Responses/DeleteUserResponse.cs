namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class DeleteUserResponse : ResponseBase
{
    public DeleteUserResponse(BusinessStatusCodeEnum statusCode, string messageText)
        : base(statusCode, messageText)
    {

    }
};
