using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging;

public abstract class SnapshotRequestBase
{
    public User User { get; set; }

    public SnapshotRequestBase(User user)
    {
        User = user;
    }
};
