using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetSnapshotsRequest : SnapshotRequestBase
{
    public GetSnapshotsRequest(User user) : base(user)
    {

    }
};
