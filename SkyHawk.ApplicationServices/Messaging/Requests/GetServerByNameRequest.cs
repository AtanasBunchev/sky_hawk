using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetServerByNameRequest : ServerRequestBase
{
    public string Name { get; set; }

    public GetServerByNameRequest(int userId, string name) : base(userId)
    {
        Name = name;
    }
};
