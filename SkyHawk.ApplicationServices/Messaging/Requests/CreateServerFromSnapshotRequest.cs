using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class CreateServerFromSnapshotRequest : ServerRequestBase
{
    public int SnapshotId { get; set; }
    public int Port { get; set; }
    public string Name { get; set; } = "Unnamed server";
    public string Description { get; set; } = "No description";

    public CreateServerFromSnapshotRequest(int userId, int snapshotId, int port) : base(userId)
    {
        SnapshotId = snapshotId;
        Port = port;
    }
};
