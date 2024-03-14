namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class ListUsersRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; } = -1;

    public ListUsersRequest()
    {
    
    }

    public ListUsersRequest(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
};
