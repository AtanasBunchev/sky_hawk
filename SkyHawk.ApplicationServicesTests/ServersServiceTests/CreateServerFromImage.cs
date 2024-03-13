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
    private void TestCreateServerFromImage_SetupDockerMock(CreateServerFromImageRequest request)
    {
        /*
         * I did my best and spent 2+ hours trying to get this to work.
         * I couldn't get the Strict mode to run with Docker.DotNet.
         *
         * Currently sufficed with a recursive mock.
         * I'll most likely not be mocking the docker lib much more than that.
         *
        var defaults = ServerDefaults.Get(request.Type);

        _docker.Setup(x => x.Images.CreateImageAsync(
            It.IsAny<ImagesCreateParameters>(),
            null,
            It.IsAny<Progress<JSONMessage>>(),
            It.IsAny<CancellationToken>()
        ));

        Dictionary<string, EmptyStruct> exposedPorts = new();
        string protocol = defaults.Protocol == PortProtocol.UDP ? "udp" : "tcp";
        exposedPorts.Add($"{request.Port}:{defaults.InternalPort}/{protocol}", default);

        _docker.Setup(x => x.Containers.CreateContainerAsync(
            new CreateContainerParameters()
            {
                Image = $"{defaults.Image}:{defaults.Tag}",
                HostConfig = It.IsAny<HostConfig>(),
                Env = defaults.Env,
                //ExposedPorts = exposedPorts
                ExposedPorts = It.IsAny<IDictionary<string, EmptyStruct>>()
            },
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(new CreateContainerResponse() {
            ID = new String('0', 64),
            Warnings = new List<string>()
        });
        //)).ReturnsAsync(new CreateContainerResponse()).SetupProperty(x => x.ID).Returns(new String('0', 64));
        */

        CreateContainerResponse result = new() { ID = new String('0', 64)};
        _docker.Setup(x => x.Containers.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(result);
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

        Assert.Equal(type, server.Type);
        Assert.Equal(port, server.Port);
        Assert.Equal(name, server.Name);
        Assert.Equal(description, server.Description);
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
