using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class DeleteServerRequest : ServerRequestBase
{
    public int Id { get; set; }

    public DeleteServerRequest(int userId, int id) : base(userId)
    {
        Id = id;
    }
};
