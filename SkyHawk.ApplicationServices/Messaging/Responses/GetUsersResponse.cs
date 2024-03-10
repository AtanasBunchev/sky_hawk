using SkyHawk.Data.Entities;
using System.Collections.Generic;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetUsersResponse : ResponseBase
{
    public ICollection<User> Users { get; set; }

    public GetUsersResponse(ICollection<User> users, BusinessStatusCodeEnum statusCode = BusinessStatusCodeEnum.Success)
    {
        Users = users;
        StatusCode = statusCode;
    }
};
