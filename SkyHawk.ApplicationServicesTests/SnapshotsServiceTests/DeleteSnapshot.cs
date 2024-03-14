using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;
using Moq;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests : IDisposable
{
    private void TestDeleteSnapshot_SetupDockerMock(DeleteSnapshotRequest request, Snapshot snapshot)
    {
        IList<IDictionary<string, string>> deleteResult = new List<IDictionary<string, string>>();
        _docker
            .Setup(
                x => x.Images.DeleteImageAsync(
                    snapshot.ImageId,
                    It.IsAny<ImageDeleteParameters>(),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(deleteResult)
            .Verifiable(); // Ty Pan hsttps://stackoverflow.com/questions/21253523/
    }

    [Fact]
    public async void TestDeleteSnapshot()
    {
        var snapshot = GetValidSnapshotEntity();
        _context.Add(snapshot);
        _context.SaveChanges();
        int id = snapshot.Id;

        DeleteSnapshotRequest request = new (_user, id);
        TestDeleteSnapshot_SetupDockerMock(request, snapshot);
        var response = await _service.DeleteSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Empty(_context.Snapshots.ToList());

        response = await _service.DeleteSnapshotAsync(new(_user, id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
