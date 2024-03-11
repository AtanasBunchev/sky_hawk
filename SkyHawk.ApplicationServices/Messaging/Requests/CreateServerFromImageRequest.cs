using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class CreateServerFromImageRequest : ServerRequestBase
{
    public CreateServerFromImageRequest(User user) : base(user)
    {
        
    }
};
