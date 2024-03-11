using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests
{
    [Fact]
    public async void TestDeleteSnapshot()
    {
        var snapshot = GetValidSnapshotEntity();
        _context.Add(snapshot);
        _context.SaveChanges();
        int id = snapshot.Id;

        var response = await _service.DeleteSnapshotAsync(new(_user, id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Empty(_context.Snapshots.ToList());

        response = await _service.DeleteSnapshotAsync(new(_user, id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
