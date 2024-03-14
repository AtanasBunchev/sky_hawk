using SkyHawk.Data.Entities;
using SkyHawk.Data.Server;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using Moq;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests
{
    private void TestCreateServerFromSnapshot_SetupSnapshot(out Snapshot snapshot)
    {
        snapshot = new Snapshot{
            ImageId = new String('0', 64),
            Type = ServerType.MinetestDevTest,
            Owner = _userObject,
            Name = "Stored snapshot",
            Description = "Description"
        };
        _context.Snapshots.Add(snapshot);
        _context.SaveChanges();
    }

    private void TestCreateServerFromSnapshot_SetupDockerMock(CreateServerFromSnapshotRequest request, Snapshot snapshot)
    {
        var data = ServerDefaults.Get(snapshot.Type);

        CreateContainerResponse createResult = new() { ID = new String('0', 64) };
        _docker.Setup(
                x => x.Containers.CreateContainerAsync(
                    It.IsAny<CreateContainerParameters>(),
                    It.IsAny<CancellationToken>()
                ))
            .Callback<CreateContainerParameters, CancellationToken>
                ((p, _) => {
                    Assert.Equal($"{snapshot.ImageId}", p.Image);
                    Assert.Equal(data.Env, p.Env);
                    string protocol = data.Protocol == PortProtocol.UDP ? "udp" : "tcp";
                    Assert.Equal($"{request.Port}", p.HostConfig.PortBindings[$"{data.InternalPort}/{protocol}"][0].HostPort);
                })
            .ReturnsAsync(createResult);
    }

    [Fact]
    public async void TestCreateServerFromSnapshot_CreatesServer()
    {
        TestCreateServerFromSnapshot_SetupSnapshot(out Snapshot snapshot);
        int port = 30512;
        string name = "New server";
        string description = "Description";

        CreateServerFromSnapshotRequest request = new(_user, snapshot.Id, port) {
            Name = name,
            Description = description
        };

        TestCreateServerFromSnapshot_SetupDockerMock(request, snapshot);

        var response = await _service.CreateServerFromSnapshotAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        var server = _context.Servers.FirstOrDefault();
        Assert.NotNull(server);

        Assert.Equal(snapshot.Type, server.Type);
        Assert.Equal(port, server.Port);
        Assert.Equal(name, server.Name);
        Assert.Equal(description, server.Description);
    }

    [Fact]
    public async void TestCreateServerFromSnapshot_InvalidType_Fails()
    {
        TestCreateServerFromSnapshot_SetupSnapshot(out Snapshot snapshot);
        snapshot.Type = ServerType.Unknown;
        await _context.SaveChangesAsync();
        int port = 30512;

        var response = await _service
            .CreateServerFromSnapshotAsync(new(_user, snapshot.Id, port));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }

    [Fact]
    public async void TestCreateServerFromSnapshot_TooLowPort_Fails()
    {
        TestCreateServerFromSnapshot_SetupSnapshot(out Snapshot snapshot);
        int port = 1000;

        var response = await _service
            .CreateServerFromSnapshotAsync(new(_user, snapshot.Id, port));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }

    [Fact]
    public async void TestCreateServerFromSnapshot_InvalidName_Fails()
    {
        TestCreateServerFromSnapshot_SetupSnapshot(out Snapshot snapshot);
        int port = 30512;
        string name = new String('n', 512); // too long

        var response = await _service
            .CreateServerFromSnapshotAsync(new(_user, snapshot.Id, port) { Name = name });
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }
}

