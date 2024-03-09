using SkyHawk.Data.Server;

namespace SkyHawk.Data.Entities;

public class Snapshot : Entity
{
    public string ImageId { get; set; } = null!;
    public ServerType Type { get; set; } = ServerType.Unknown;

    public User Owner { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreateTime { get; set; }
}
