using SkyHawk.Data.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyHawk.Data.Entities;

public class ServerInstance : Entity
{
    [StringLength(64)]
    public string ContainerId { get; set; } = null!;
    public ServerType Type { get; set; } = ServerType.Unknown;
    public int Port { get; set; } = -1;

    public User Owner { get; set; } = null!;

    [MaxLength(64)]
    public string Name { get; set; } = null!;
    [MaxLength(1024)]
    public string Description { get; set; } = null!;
    public byte[]? Image { get; set; } = null;

    public TimeOnly? AutoStart { get; set; } = null;
    public TimeOnly? AutoStop { get; set; } = null;

    [InverseProperty("Server")]
    public List<Activity> Activities { get; set; } = new();
}
