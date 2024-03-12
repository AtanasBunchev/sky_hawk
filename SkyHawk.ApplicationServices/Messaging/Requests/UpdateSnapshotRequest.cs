using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateSnapshotRequest : SnapshotRequestBase
{
    public int SnapshotId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public UpdateSnapshotRequest(User user, int snapshotId) : base(user)
    {
        SnapshotId = snapshotId;
    }
};
