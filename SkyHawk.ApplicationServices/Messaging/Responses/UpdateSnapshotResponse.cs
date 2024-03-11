namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class UpdateSnapshotResponse : ResponseBase
{
    public UpdateSnapshotResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
