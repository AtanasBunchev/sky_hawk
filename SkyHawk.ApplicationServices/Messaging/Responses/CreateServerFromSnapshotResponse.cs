namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class CreateServerFromSnapshotResponse : ResponseBase
{
    public CreateServerFromSnapshotResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
