namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetServerResponse : ResponseBase
{
    public GetServerResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
