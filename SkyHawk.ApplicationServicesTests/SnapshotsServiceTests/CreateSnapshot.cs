using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;
using SkyHawk.Data.Server;
using Moq;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests : IDisposable
{
    private void TestCreateSnapshot_SetupDockerMock(CreateSnapshotRequest request, ServerInstance server, string snapshotImageId)
    {
        CommitContainerChangesResponse response = new () { ID = snapshotImageId };
        _docker
            .Setup(
                x => x.Images.CommitContainerChangesAsync(
                    It.IsAny<CommitContainerChangesParameters>(),
                    It.IsAny<CancellationToken>()
                ))
            .Callback<CommitContainerChangesParameters, CancellationToken>(
                (p, _) => Assert.Equal(server.ContainerId, p.ContainerID)
            )
            .ReturnsAsync(response)
            .Verifiable();
    }

    [Fact]
    public async void TestCreateSnapshot_CreatesImage()
    {
        var server = new ServerInstance {
            ContainerId = new String('c', 64),
            Type = ServerType.MinetestDevTest,
            Owner = _userObject,
            Name = "Server",
            Description = "Description"
        };
        _context.Add(server);
        _context.SaveChanges();

        var snapshotImageId = new String('s', 64);

        CreateSnapshotRequest request = new (_user, server.Id) {
            Name = "Snapshot",
            Description = "Description"
        };
        TestCreateSnapshot_SetupDockerMock(request, server, snapshotImageId);

        var response = await _service.CreateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Single(_context.Snapshots);

        var snapshot = _context.Snapshots.FirstOrDefault();
        Assert.NotNull(snapshot);
    }

    [Fact]
    public async void TestCreateSnapshot_NoPermission_Fails()
    {
        var server = new ServerInstance {
            ContainerId = new String('c', 64),
            Type = ServerType.MinetestDevTest,
            Owner = _userObject,
            Name = "Server",
            Description = "Description"
        };
        _userObject.CanMakeSnapshots = false;
        _context.Add(server);
        await _context.SaveChangesAsync();

        var snapshotImageId = new String('s', 64);

        CreateSnapshotRequest request = new (_user, server.Id) {
            Name = "Snapshot",
            Description = "Description"
        };

        var response = await _service.CreateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NoPermission, response.StatusCode);
        Assert.Empty(_context.Snapshots);
    }

    [Fact]
    public async void TestCreateSnapshot_UserNotExists_Fails()
    {
        var server = new ServerInstance {
            ContainerId = new String('c', 64),
            Type = ServerType.MinetestDevTest,
            Owner = _userObject,
            Name = "Server",
            Description = "Description"
        };
        _context.Add(server);
        _context.SaveChanges();

        var snapshotImageId = new String('s', 64);

        CreateSnapshotRequest request = new (10, server.Id) {
            Name = "Snapshot",
            Description = "Description"
        };

        var response = await _service.CreateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Empty(_context.Snapshots);
    }

    [Fact]
    public async void TestCreateSnapshot_ServerNotExists_Fails()
    {
        var snapshotImageId = new String('s', 64);

        CreateSnapshotRequest request = new (_user, 10) {
            Name = "Snapshot",
            Description = "Description"
        };

        var response = await _service.CreateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Empty(_context.Snapshots);
    }

    [Fact]
    public async void TestCreateSnapshot_InvalidName_Fails()
    {
        var server = new ServerInstance {
            ContainerId = new String('c', 64),
            Type = ServerType.MinetestDevTest,
            Owner = _userObject,
            Name = "Server",
            Description = "Description"
        };
        _context.Add(server);
        _context.SaveChanges();

        var snapshotImageId = new String('s', 64);

        CreateSnapshotRequest request = new (_user, server.Id) {
            Name = new String('n', 512), // Too long
            Description = "Description"
        };

        var response = await _service.CreateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
        Assert.Empty(_context.Snapshots);
    }

    [Fact]
    public async void TestCreateSnapshot_OtherOwner_Fails()
    {
        var server = new ServerInstance {
            ContainerId = new String('c', 64),
            Type = ServerType.MinetestDevTest,
            Owner = _userObject,
            Name = "Server",
            Description = "Description"
        };
        var stranger = UsersServiceTests.GetValidUserEntity();

        _context.Add(server);
        _context.Add(stranger);

        _context.SaveChanges();

        var snapshotImageId = new String('s', 64);

        CreateSnapshotRequest request = new (stranger.Id, server.Id) {
            Name = "Snapshot",
            Description = "Description"
        };

        var response = await _service.CreateSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Empty(_context.Snapshots);
    }
 }
