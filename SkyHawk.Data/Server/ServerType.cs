using System.ComponentModel.DataAnnotations;

namespace SkyHawk.Data.Server;

public enum ServerType
{
    Unknown = 0,
    MinetestGame = 16,
    NodeCoreGame,
    DDNet = 32
}

