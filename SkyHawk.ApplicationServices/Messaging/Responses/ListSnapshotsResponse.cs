namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class ListSnapshotsResponse : ResponseBase
{
    public ListSnapshotsResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }
};
