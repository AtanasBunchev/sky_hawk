using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetSnapshotByIdRequest : SnapshotRequestBase
{
    public int Id { get; set; }

    public GetSnapshotByIdRequest(int userId, int id) : base(userId)
    {
        Id = id;
    }
};
