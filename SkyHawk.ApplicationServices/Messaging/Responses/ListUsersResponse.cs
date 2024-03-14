using SkyHawk.ApplicationServices.Messaging.ViewModels;
using SkyHawk.Data.Entities;
using System.Collections.Generic;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class ListUsersResponse : ResponseBase
{
    public ICollection<UserVM> Users { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = -1;

    public ListUsersResponse(ICollection<UserVM> users)
        : base(BusinessStatusCodeEnum.Success, "Users list fetched successfully.")
    {
        Users = users;
    }

    public ListUsersResponse(ICollection<UserVM> users, BusinessStatusCodeEnum statusCode)
        : base(statusCode)
    {
        Users = users;
    }

    public ListUsersResponse(ICollection<UserVM> users, BusinessStatusCodeEnum statusCode, string msg)
        : base(statusCode, msg)
    {
        Users = users;
    }

    public ListUsersResponse(ICollection<UserVM> users, int page, int pageSize)
        : base(BusinessStatusCodeEnum.Success, "Users list fetched successfully.")
    {
        Users = users;
        Page = page;
        PageSize = pageSize;
    }
};
