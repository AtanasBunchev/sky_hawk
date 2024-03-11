using System.Linq;

namespace SkyHawk.Data.Server;

public static class ServerDefaults
{
    public static ServerDefaultsBundle? Get(ServerType type)
    {
        return _defaults.SingleOrDefault(x => x.Type == type);
    }


    public class ServerDefaultsBundle
    {
        public ServerType Type;
        public string Name = null!;
        public string Image = null!;

        public PortProtocol Protocol;
        public int InternalPort;
    }

    private static List<ServerDefaultsBundle> _defaults = new List<ServerDefaultsBundle>(){
        new ServerDefaultsBundle {
            Type = ServerType.MinetestGame,
            Name = "Minetest Game Server",
            Image = "<image>",
            Protocol = PortProtocol.UDP,
            InternalPort = 30000
        },
        new ServerDefaultsBundle {
            Type = ServerType.NodeCore,
            Name = "NodeCore Server",
            Image = "<image>",
            Protocol = PortProtocol.UDP,
            InternalPort = 30000
        },
        new ServerDefaultsBundle {
            Type = ServerType.DDNet,
            Name = "DDNet Server",
            Image = "<image>",
            Protocol = PortProtocol.UDP,
            InternalPort = -1
        }
    };
}

