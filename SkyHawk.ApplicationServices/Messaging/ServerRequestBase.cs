using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging;

public abstract class ServerRequestBase
{
    public User User { get; set; }

    public ServerRequestBase(User user)
    {
        User = user;
    }
};
