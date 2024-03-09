using System.ComponentModel.DataAnnotations.Schema;

namespace SkyHawk.Data.Entities;

public class User : Entity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public DateTime CreateTime { get; set; } = DateTime.Now;

    public int MaxServers { get; set; } = 10;
    public int MaxRunningServers { get; set; } = 3;
    public bool CanMakeSnapshots { get; set; } = true;


    [InverseProperty("Owner")]
    public List<ServerInstance> Servers { get; set; } = new();

    [InverseProperty("Owner")]
    public List<Snapshot> Shapshots { get; set; } = new();
}
