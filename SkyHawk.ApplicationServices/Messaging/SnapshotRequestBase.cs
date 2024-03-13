namespace SkyHawk.ApplicationServices.Messaging;

public abstract class SnapshotRequestBase
{
    public int UserId { get; set; }

    public SnapshotRequestBase(int userId)
    {
        UserId = userId;
    }
};
