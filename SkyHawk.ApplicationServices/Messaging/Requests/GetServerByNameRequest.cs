using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetServerByNameRequest : ServerRequestBase
{
    public string Name { get; set; }

    public GetServerByNameRequest(User user, string name) : base(user)
    {
        Name = name;
    }
};
