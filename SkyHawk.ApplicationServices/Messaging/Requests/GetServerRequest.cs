using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetServerRequest : ServerRequestBase
{
    public GetServerRequest(User user) : base(user)
    {
        
    }
};
