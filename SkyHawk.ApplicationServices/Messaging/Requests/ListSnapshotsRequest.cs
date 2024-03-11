using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class ListSnapshotsRequest : SnapshotRequestBase
{
    public ListSnapshotsRequest(User user) : base(user)
    {

    }
};
