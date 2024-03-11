namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class UpdateServerImageResponse : ResponseBase
{
    public UpdateServerImageResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
