using SkyHawk.Data.Entities;
using System.Collections.Generic;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetUsersResponse : ResponseBase
{
    public ICollection<User> Users { get; set; }

    public GetUsersResponse(ICollection<User> users)
        : base(BusinessStatusCodeEnum.Success, "Users list fetched successfully.")
    {
        Users = users;
    }

    public GetUsersResponse(ICollection<User> users, BusinessStatusCodeEnum statusCode)
        : base(statusCode)
    {
        Users = users;
    }

    public GetUsersResponse(ICollection<User> users, BusinessStatusCodeEnum statusCode, string msg)
        : base(statusCode, msg)
    {
        Users = users;
    }
};
