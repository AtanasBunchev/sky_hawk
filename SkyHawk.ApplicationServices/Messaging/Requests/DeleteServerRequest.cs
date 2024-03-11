using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class DeleteServerRequest : ServerRequestBase
{
    public DeleteServerRequest(User user) : base(user)
    {
        
    }
};
