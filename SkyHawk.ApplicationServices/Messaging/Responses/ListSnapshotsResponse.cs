using SkyHawk.Data.Entities;
using System.Collections.Generic;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class ListSnapshotsResponse : ResponseBase
{
    public ICollection<Snapshot> Snapshots { get; set; }

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
};
