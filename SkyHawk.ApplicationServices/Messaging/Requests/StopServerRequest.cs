using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class StopServerRequest : ServerRequestBase
{
    public int ServerId { get; set; }

    public StopServerRequest(int userId, int serverId) : base(userId)
    {
        ServerId = serverId;
    }
};
