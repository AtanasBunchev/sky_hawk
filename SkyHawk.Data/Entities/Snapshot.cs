using SkyHawk.Data.Server;
using System.ComponentModel.DataAnnotations;

namespace SkyHawk.Data.Entities;

public class Snapshot : Entity
{
    [StringLength(256)]
    public string ImageId { get; set; } = null!;
    public ServerType Type { get; set; } = ServerType.Unknown;

    public User Owner { get; set; } = null!;

    [MaxLength(64)]
    public string Name { get; set; } = null!;
    [MaxLength(1024)]
    public string Description { get; set; } = null!;
    public DateTime CreateTime { get; set; } = DateTime.Now;
}
