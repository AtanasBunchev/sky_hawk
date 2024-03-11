using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateServerImageRequest : ServerRequestBase
{
    public UpdateServerImageRequest(User user) : base(user)
    {
        
    }
};
