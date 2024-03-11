using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateSnapshotRequest : SnapshotRequestBase
{
    public UpdateSnapshotRequest(User user) : base(user)
    {

    }
};
