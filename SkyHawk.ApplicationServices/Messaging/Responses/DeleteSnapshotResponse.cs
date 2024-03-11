namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class DeleteSnapshotResponse : ResponseBase
{
    public DeleteSnapshotResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
