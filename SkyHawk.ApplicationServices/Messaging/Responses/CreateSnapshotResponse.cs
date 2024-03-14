namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class CreateSnapshotResponse : ResponseBase
{
    public int SnapshotId { get; set; } = -1;

    public CreateSnapshotResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }

    public CreateSnapshotResponse(int serverId)
        : base(BusinessStatusCodeEnum.Success, "Snapshot created successfully.")
    {
        SnapshotId = serverId;
    }
};
