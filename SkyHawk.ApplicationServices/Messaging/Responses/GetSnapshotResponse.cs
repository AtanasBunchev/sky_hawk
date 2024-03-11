namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetSnapshotResponse : ResponseBase
{
    public GetSnapshotResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
