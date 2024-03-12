namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateUserRequest
{
    public int Id { get; set; }

    public string? Username { get; set; } = null;
    public string? Password { get; set; } = null;

/*
    public int? MaxServers { get; set; } = null;
    public int? MaxRunningServers { get; set; } = null;
    public bool? CanMakeSnapshotsServers { get; set; } = null;
*/

    public UpdateUserRequest(int id, string? username = null, string? password = null)
    {
        Id = id;
        Username = username;
        Password = password;
    }
};
