namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class StartServerResponse : ResponseBase
{
    public StartServerResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
