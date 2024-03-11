namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class ListServersResponse : ResponseBase
{
    public ListServersResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
