using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class CreateServerFromSnapshotRequest : ServerRequestBase
{
    public CreateServerFromSnapshotRequest(int userId) : base(userId)
    {
        
    }
};
