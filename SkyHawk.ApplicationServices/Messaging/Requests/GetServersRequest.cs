using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetServersRequest : ServerRequestBase
{
    public GetServersRequest(User user) : base(user)
    {
        
    }
};
