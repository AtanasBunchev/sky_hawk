namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class UpdateServerPortResponse : ResponseBase
{
    public UpdateServerPortResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
