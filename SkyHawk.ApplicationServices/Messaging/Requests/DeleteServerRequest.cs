using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class DeleteServerRequest : ServerRequestBase
{
    public int Id { get; set; }

    public DeleteServerRequest(User user, int id) : base(user)
    {
        Id = id;
    }
};
