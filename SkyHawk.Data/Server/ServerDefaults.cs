using System.Linq;
using System.Collections.Generic;

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
        public string Tag = null!;

        public PortProtocol Protocol;
        public int InternalPort;

        public List<String> Env = null!;
    }

    private static List<ServerDefaultsBundle> _defaults = new List<ServerDefaultsBundle>(){
        new ServerDefaultsBundle {
            Type = ServerType.MinetestDevTest,
            Name = "Minetest Devtest Server",
            Image = "lscr.io/linuxserver/minetest",
            Tag = "latest",

            Protocol = PortProtocol.UDP,
            InternalPort = 30000,

            Env = new List<String> {
                "PUID=1000",
                "PGID=1000",
                "TZ=Etc/UTC",
                "CLI_ARGS=\"--gameid devtest --port 30000\""
            }
        }/*,
        new ServerDefaultsBundle {
            Type = ServerType.MinetestGame,
            Name = "Minetest Devtest Server",
            Image = "lscr.io/linuxserver/minetest",
            Tag = "latest",
            Protocol = PortProtocol.UDP,
            InternalPort = 30000
        },
        new ServerDefaultsBundle {
            Type = ServerType.NodeCore,
            Name = "NodeCore Server",
            Image = "<image>",
            Tag = "latest",
            Protocol = PortProtocol.UDP,
            InternalPort = 30000
        },
        new ServerDefaultsBundle {
            Type = ServerType.DDNet,
            Name = "DDNet Server",
            Image = "<image>",
            Tag = "latest",
            Protocol = PortProtocol.UDP,
            InternalPort = -1
        }
        */
    };
}

