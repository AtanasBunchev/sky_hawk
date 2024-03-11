using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetSnapshotRequest : SnapshotRequestBase
{
    public GetSnapshotRequest(User user) : base(user)
    {

    }
};
