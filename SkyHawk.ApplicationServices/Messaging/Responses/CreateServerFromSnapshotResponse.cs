namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class CreateServerFromSnapshotResponse : ResponseBase
{
    public int ServerId { get; set; } = -1;

    public CreateServerFromSnapshotResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }

    public CreateServerFromSnapshotResponse(int serverId)
        : base(BusinessStatusCodeEnum.Success, "Server created successfully.")
    {
        ServerId = serverId;
    }
};
