namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class StopServerResponse : ResponseBase
{
    public StopServerResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
