namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class ControlServerResponse : ResponseBase
{
    public ControlServerResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
