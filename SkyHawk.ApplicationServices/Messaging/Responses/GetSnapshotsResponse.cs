namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetSnapshotsResponse : ResponseBase
{
    public GetSnapshotsResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
