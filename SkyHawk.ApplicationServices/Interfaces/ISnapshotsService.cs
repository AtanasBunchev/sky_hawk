using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;

namespace SkyHawk.ApplicationServices.Interfaces;

public interface ISnapshotsService
{
    // List snapshots
    public Task<ListSnapshotsResponse> ListSnapshotsAsync(ListSnapshotsRequest request);

    // Get snapshot
    public Task<GetSnapshotResponse> GetSnapshotAsync(GetByIdRequest request);
    public Task<GetSnapshotResponse> GetSnapshotAsync(GetByNameRequest request);

    // Create snapshot
    public Task<CreateSnapshotResponse> CreateSnapshotAsync(CreateSnapshotRequest request);

    // Modify snapshot
    public Task<UpdateSnapshotResponse> UpdateSnapshotAsync(UpdateSnapshotRequest request);

    // Delete snapshot
    public Task<DeleteSnapshotResponse> DeleteSnapshotAsync(DeleteSnapshotRequest request);
}