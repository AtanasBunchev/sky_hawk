using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetServerByIdRequest : ServerRequestBase
{
    public int Id { get; set; }

    public GetServerByIdRequest(int userId, int id) : base(userId)
    {
        Id = id;
    }
};
