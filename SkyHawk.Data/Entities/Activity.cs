using SkyHawk.Data.Server;
using System.ComponentModel.DataAnnotations;

namespace SkyHawk.Data.Entities;

public enum StartReasonEnum
{
    Unknown = 0,
    Timer = 8,
    Manual = 16
}

public class Activity : Entity
{
    public ServerInstance Server { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public StartReasonEnum StartReason = StartReasonEnum.Unknown;

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    //public string Logs { get; set; } = null!;
}
