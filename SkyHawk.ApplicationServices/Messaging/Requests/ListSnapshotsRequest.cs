using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class ListSnapshotsRequest : SnapshotRequestBase
{
    public int Page { get; set; }
    public int PageSize { get; set; } = -1;

    public ListSnapshotsRequest(int userId) : base(userId)
    {

    }

    public ListSnapshotsRequest(int userId, int page, int pageSize) : base(userId)
    {
        Page = page;
        PageSize = pageSize;
    }
};
