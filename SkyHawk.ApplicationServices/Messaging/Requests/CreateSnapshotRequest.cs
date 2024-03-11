using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class CreateSnapshotRequest : SnapshotRequestBase
{
    public CreateSnapshotRequest(User user) : base(user)
    {

    }
};
