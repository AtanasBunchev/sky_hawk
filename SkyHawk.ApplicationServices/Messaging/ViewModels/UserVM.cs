namespace SkyHawk.ApplicationServices.Messaging.ViewModels;

public class UserVM
{
    public int Id { get; set; }

    public string Username { get; set; }
    public DateTime CreateTime { get; set; }

    public int MaxServers { get; set; }
    public int MaxRunningServers { get; set; }
    public bool CanMakeSnapshots { get; set; }
}

