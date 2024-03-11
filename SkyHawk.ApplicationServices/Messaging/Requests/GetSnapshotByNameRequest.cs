using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetSnapshotByNameRequest : SnapshotRequestBase
{
    public string Name { get; set; }

    public GetSnapshotByNameRequest(User user, string name) : base(user)
    {
        Name = name;
    }
};
