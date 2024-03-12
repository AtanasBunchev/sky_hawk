using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests
{
    [Fact]
    public async void TestUpdateSnapshot_ValidData_Succeeds()
    {
        var snapshot = GetValidSnapshotEntity();
        _context.Snapshots.Add(snapshot);
        _context.SaveChanges();

        for(int i = 0; i < (1 << 6) - 1; i++) {
            UpdateSnapshotRequest request = new (_user, snapshot.Id);

            if((i & 1) == 1) {
                request.Name = $"Test {i}";
            }
            if((i & 2) == 2) {
                request.Description = $"Test {i} runs now";
            }

            var response = await _service.UpdateSnapshotAsync(request);
            Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

            if(request.Name != null)
                Assert.Equal(request.Name, snapshot.Name);
            if(request.Description != null)
                Assert.Equal(request.Description, snapshot.Description);
        }
    }

    [Fact]
    public async void TestUpdateSnapshot_InvalidName_Fails()
    {
        var snapshot = GetValidSnapshotEntity();
        _context.Snapshots.Add(snapshot);
        _context.SaveChanges();

        var invalidName = new String('u', 512); // Too Long
        var originalName = snapshot.Name;

        UpdateSnapshotRequest request = new (_user, snapshot.Id) {
            Name = invalidName
        };
        var response = await _service.UpdateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
        Assert.Equal(originalName, snapshot.Name);
    }

    [Fact]
    public async void TestUpdateSnapshot_NotExists_Fails()
    {
        UpdateSnapshotRequest request = new (_user, 10);
        var response = await _service.UpdateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
