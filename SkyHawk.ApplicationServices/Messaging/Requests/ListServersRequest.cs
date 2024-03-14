using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class ListServersRequest : ServerRequestBase
{
    public int Page { get; set; }
    public int PageSize { get; set; } = -1;

    public ListServersRequest(int userId) : base(userId)
    {
        
    }

    public ListServersRequest(int userId, int page, int pageSize) : base(userId)
    {
        Page = page;
        PageSize = pageSize;
    }
};
