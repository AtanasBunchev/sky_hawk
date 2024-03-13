using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class ControlServerRequest : ServerRequestBase
{
    public ControlServerRequest(int userId) : base(userId)
    {
        
    }
};
