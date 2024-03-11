using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetSnapshotByIdRequest : SnapshotRequestBase
{
    public int Id { get; set; }

    public GetSnapshotByIdRequest(User user, int id) : base(user)
    {
        Id = id;
    }
};
