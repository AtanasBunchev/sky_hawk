using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class DeleteSnapshotRequest : SnapshotRequestBase
{
    public int Id { get; set; }

    public DeleteSnapshotRequest(int userId, int id) : base(userId)
    {
        Id = id;
    }
};
