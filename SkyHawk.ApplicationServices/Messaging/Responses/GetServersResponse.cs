namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetServersResponse : ResponseBase
{
    public GetServersResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
