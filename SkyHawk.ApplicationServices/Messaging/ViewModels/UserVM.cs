namespace SkyHawk.ApplicationServices.Messaging.ViewModels;

#nullable disable

public class UserVM
{
    public int Id { get; set; }

    public string Username { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime LastLogin { get; set; }

    public int MaxServers { get; set; }
    public bool CanMakeSnapshots { get; set; }
}

