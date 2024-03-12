using SkyHawk.Data.Entities;
using SkyHawk.Data.Server;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class CreateServerFromImageRequest : ServerRequestBase
{
    public ServerType Type { get; set; }
    public int Port { get; set; }
    public string Name { get; set; } = "Unnamed server";
    public string Description { get; set; } = "No description";

    public CreateServerFromImageRequest(User user, ServerType type, int port) : base(user)
    {
        Type = type;
        Port = port;
    }
};
