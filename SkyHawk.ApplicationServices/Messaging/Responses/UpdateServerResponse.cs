namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class UpdateServerResponse : ResponseBase
{
    public UpdateServerResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
