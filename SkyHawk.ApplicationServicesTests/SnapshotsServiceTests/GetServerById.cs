using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests : IDisposable
{
    [Fact]
    public async void TestGetSnapshotById()
    {
        var snapshot = GetValidSnapshotEntity();

        var nullResponse = await _service.GetSnapshotByIdAsync(new(_user, 10));
        Assert.Null(nullResponse.Snapshot);

        _context.Snapshots.Add(snapshot);
        _context.SaveChanges();

        var response = await _service.GetSnapshotByIdAsync(new(_user, snapshot.Id));

        Assert.Equal(snapshot, response.Snapshot);
    }
}
