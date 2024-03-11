using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateServerRequest : ServerRequestBase
{
    public UpdateServerRequest(User user) : base(user)
    {
        
    }
};
