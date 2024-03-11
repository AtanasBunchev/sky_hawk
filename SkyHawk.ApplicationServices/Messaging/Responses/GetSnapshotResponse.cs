using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetSnapshotResponse : ResponseBase
{
    public Snapshot? Snapshot { get; set; }

    public GetSnapshotResponse(Snapshot? snapshot)
        : base(BusinessStatusCodeEnum.None)
    {
        Snapshot = snapshot;
        if (snapshot == null) {
            StatusCode = BusinessStatusCodeEnum.NotFound;
            MessageText = "Snapshot not found!";
        } else {
            StatusCode = BusinessStatusCodeEnum.Success;
            MessageText = "Snapshot fetched successfully.";
        }
    }
};
