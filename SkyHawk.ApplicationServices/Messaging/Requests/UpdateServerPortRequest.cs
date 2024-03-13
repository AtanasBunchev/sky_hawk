using SkyHawk.Data.Entities;
using System.IO;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateServerPortRequest : ServerRequestBase
{
    public int ServerId { get; set; }
    public int Port { get; set; }

    public UpdateServerPortRequest(int userId, int serverId, int port) : base(userId)
    {
        ServerId = serverId;
        Port = port;
    }
};
