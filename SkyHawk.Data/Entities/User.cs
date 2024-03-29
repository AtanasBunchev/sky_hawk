using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyHawk.Data.Entities;

public class User : Entity
{
    [MaxLength(64)]
    [MinLength(3)]
    public string Username { get; set; } = null!;
    [MaxLength(128)]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    public DateTime CreateTime { get; set; } = DateTime.Now;
    public DateTime LastLogin { get; set; } = DateTime.Now;

    public int MaxServers { get; set; } = 10;
    public bool CanMakeSnapshots { get; set; } = true;


    [InverseProperty("Owner")]
    public List<ServerInstance> Servers { get; set; } = new();

    [InverseProperty("Owner")]
    public List<Snapshot> Shapshots { get; set; } = new();
}
