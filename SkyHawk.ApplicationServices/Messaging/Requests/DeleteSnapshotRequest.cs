using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class DeleteSnapshotRequest : SnapshotRequestBase
{
    public int Id { get; set; }

    public DeleteSnapshotRequest(User user, int id) : base(user)
    {
        Id = id;
    }
};
