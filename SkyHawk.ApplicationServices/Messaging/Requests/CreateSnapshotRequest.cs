using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class CreateSnapshotRequest : SnapshotRequestBase
{
    public int ServerId { get; set; }
    public string Name { get; set; } = "Snapshot";
    public string Description { get; set; } = "No description";

    public CreateSnapshotRequest(int userId, int serverId) : base(userId)
    {
        ServerId = serverId;
    }
};
