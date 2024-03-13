using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetSnapshotByNameRequest : SnapshotRequestBase
{
    public string Name { get; set; }

    public GetSnapshotByNameRequest(int userId, string name) : base(userId)
    {
        Name = name;
    }
};
