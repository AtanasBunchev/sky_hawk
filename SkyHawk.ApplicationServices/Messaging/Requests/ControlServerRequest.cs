using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class ControlServerRequest : ServerRequestBase
{
    public ControlServerRequest(User user) : base(user)
    {
        
    }
};
