using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests
{
    [Fact]
    public async void TestGetSnapshotByName()
    {
        var snapshot = GetValidSnapshotEntity();

        var nullResponse = await _service.GetSnapshotByNameAsync(new(_user, snapshot.Name));
        Assert.Null(nullResponse.Snapshot);

        _context.Snapshots.Add(snapshot);
        _context.SaveChanges();

        var response = await _service.GetSnapshotByNameAsync(new(_user, snapshot.Name));
        Assert.Equal(snapshot, response.Snapshot);
    }
}
