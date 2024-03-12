using SkyHawk.Data.Entities;
using System.IO;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateServerRequest : ServerRequestBase
{
    public int ServerId { get; set; }
    public int? Port { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public Stream? Image { get; set; }

    // Set these equal to clean them up
    public TimeOnly? AutoStart { get; set; }
    public TimeOnly? AutoStop { get; set; }

    public UpdateServerRequest(User user, int serverId) : base(user)
    {
        ServerId = serverId;
    }
};
