namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class DeleteServerResponse : ResponseBase
{
    public DeleteServerResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {
        
    }
};
