namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class CreateSnapshotResponse : ResponseBase
{
    public CreateSnapshotResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
