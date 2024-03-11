using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class ListServersRequest : ServerRequestBase
{
    public ListServersRequest(User user) : base(user)
    {
        
    }
};
