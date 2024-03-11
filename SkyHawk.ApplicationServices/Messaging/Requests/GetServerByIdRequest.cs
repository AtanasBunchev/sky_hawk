using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetServerByIdRequest : ServerRequestBase
{
    public int Id { get; set; }

    public GetServerByIdRequest(User user, int id) : base(user)
    {
        Id = id;
    }
};
