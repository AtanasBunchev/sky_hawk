using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests : IDisposable
{
    [Fact]
    public async void TestListSnapshots()
    {
        var emptyResponse = await _service.ListSnapshotsAsync(new(_user));
        Assert.Empty(emptyResponse.Snapshots);

        var snapshot = GetValidSnapshotEntity();
        _context.Snapshots.Add(snapshot);
        _context.SaveChanges();

        var response = await _service.ListSnapshotsAsync(new(_user));

        Assert.NotEmpty(response.Snapshots);
        Assert.Single(response.Snapshots);
        foreach(var returnedSnapshot in response.Snapshots)
            Assert.Equal(snapshot, returnedSnapshot);
    }
}
