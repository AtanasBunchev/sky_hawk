namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class CreateServerFromImageResponse : ResponseBase
{
    public CreateServerFromImageResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
