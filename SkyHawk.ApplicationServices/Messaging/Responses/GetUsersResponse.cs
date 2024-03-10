using SkyHawk.Data.Entities;
using System.Collections.Generic;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetUsersResponse
{
    public ICollection<User> Users { get; set; }

    public GetUsersResponse(ICollection<User> users)
    {
        Users = users;
    }
};
