namespace SkyHawk.ApplicationServices.Messaging;

public abstract class ServerRequestBase
{
    public int UserId { get; set; }

    public ServerRequestBase(int userId)
    {
        UserId = userId;
    }
};
