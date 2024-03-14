using SkyHawk.Data.Entities;
using SkyHawk.Data.Server;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using Moq;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests : IDisposable
{
    private void TestCreateServerFromImage_SetupDockerMock(CreateServerFromImageRequest request)
    {
        var data = ServerDefaults.Get(request.Type);
        Assert.NotNull(data);

        _docker.Setup(x => x.Images.CreateImageAsync(
                It.IsAny<ImagesCreateParameters>(),
                It.IsAny<AuthConfig>(),
                It.IsAny<IProgress<JSONMessage>>(),
                It.IsAny<CancellationToken>()
            ))
            .Callback<ImagesCreateParameters, AuthConfig, IProgress<JSONMessage>, CancellationToken>
                ((p, _, _, _) => {
                    Assert.Equal(data.Image, p.FromImage);
                    Assert.Equal(data.Tag, p.Tag);
                })
            .Returns(Task.FromResult(default(object)))
            .Verifiable();

        CreateContainerResponse createResult = new() { ID = new String('0', 64) };
        _docker.Setup(
                x => x.Containers.CreateContainerAsync(
                    It.IsAny<CreateContainerParameters>(),
                    It.IsAny<CancellationToken>()
                ))
            .Callback<CreateContainerParameters, CancellationToken>
                ((p, _) => {
                    Assert.NotNull(p.Image);
                    Assert.Equal($"{data.Image}:{data.Tag}", p.Image);
                    Assert.NotNull(p.Env);
                    Assert.Equal(data.Env, p.Env);
                    string protocol = data.Protocol == PortProtocol.UDP ? "udp" : "tcp";
                    Assert.Equal($"{request.Port}", p.HostConfig.PortBindings[$"{data.InternalPort}/{protocol}"][0].HostPort);
                })
            .ReturnsAsync(createResult)
            .Verifiable();
    }

    [Fact]
    public async void TestCreateServerFromImage_CreatesServer()
    {
        ServerType type = ServerType.MinetestDevTest;
        int port = 30512;
        string name = "New server";
        string description = "Description";

        CreateServerFromImageRequest request = new(_user, type, port) {
            Name = name,
            Description = description
        };

        TestCreateServerFromImage_SetupDockerMock(request);

        var response = await _service.CreateServerFromImageAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        var server = _context.Servers.FirstOrDefault();
        Assert.NotNull(server);

        Assert.Equal(server.Id, response.ServerId);
        Assert.Equal(type, server.Type);
        Assert.Equal(port, server.Port);
        Assert.Equal(name, server.Name);
        Assert.Equal(description, server.Description);
    }

    [Fact]
    public async void TestCreateServerFromImage_OverLimit_Fails()
    {
        ServerType type = ServerType.MinetestDevTest;
        int port = 30512;
        string name = "New server";
        string description = "Description";

        CreateServerFromImageRequest request = new(_user, type, port) {
            Name = name,
            Description = description
        };

        TestCreateServerFromImage_SetupDockerMock(request);

        CreateServerFromImageResponse response;
        do {
            response = await _service.CreateServerFromImageAsync(request);
        } while(_context.Servers.ToList().Count <= _userObject.MaxServers && BusinessStatusCodeEnum.Success == response.StatusCode);

        Assert.Equal(BusinessStatusCodeEnum.PermittedLimitReached, response.StatusCode);

        Assert.Equal(_userObject.MaxServers, _context.Servers.ToList().Count);
    }

    [Fact]
    public async void TestCreateServerFromImage_InvalidType_Fails()
    {
        ServerType type = ServerType.Unknown;
        int port = 30512;

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }

    [Fact]
    public async void TestCreateServerFromImage_TooLowPort_Fails()
    {
        ServerType type = ServerType.MinetestDevTest;
        int port = 1000;

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }

    [Fact]
    public async void TestCreateServerFromImage_InvalidName_Fails()
    {
        ServerType type = ServerType.MinetestDevTest;
        int port = 30512;
        string name = new String('n', 512); // too long

        var response = await _service
            .CreateServerFromImageAsync(new(_user, type, port) { Name = name });
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }
}
