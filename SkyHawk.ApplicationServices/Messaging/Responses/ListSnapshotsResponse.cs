using SkyHawk.Data.Entities;
using System.Collections.Generic;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class ListSnapshotsResponse : ResponseBase
{
    public ICollection<Snapshot> Snapshots { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = -1;

    public ListSnapshotsResponse(ICollection<Snapshot> snapshots)
        : base(BusinessStatusCodeEnum.Success, "Snapshots list fetched successfully.")
    {
        Snapshots = snapshots;
    }

    public ListSnapshotsResponse(ICollection<Snapshot> snapshots, BusinessStatusCodeEnum statusCode)
        : base(statusCode)
    {
        Snapshots = snapshots;
    }

    public ListSnapshotsResponse(ICollection<Snapshot> snapshots, BusinessStatusCodeEnum statusCode, string msg)
        : base(statusCode, msg)
    {
        Snapshots = snapshots;
    }

    public ListSnapshotsResponse(ICollection<Snapshot> snapshots, int page, int pageSize)
        : base(BusinessStatusCodeEnum.Success, "Snapshots list fetched successfully.")
    {
        Snapshots = snapshots;
        Page = page;
        PageSize = pageSize;
    }
};
