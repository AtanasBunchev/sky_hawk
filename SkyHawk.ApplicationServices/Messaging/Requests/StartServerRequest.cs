using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class StartServerRequest : ServerRequestBase
{
    public int ServerId { get; set; }

    public StartServerRequest(int userId, int serverId) : base(userId)
    {
        ServerId = serverId;
    }
};
