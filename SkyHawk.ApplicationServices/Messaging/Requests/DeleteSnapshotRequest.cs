using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class DeleteSnapshotRequest : SnapshotRequestBase
{
    public DeleteSnapshotRequest(User user) : base(user)
    {

    }
};
