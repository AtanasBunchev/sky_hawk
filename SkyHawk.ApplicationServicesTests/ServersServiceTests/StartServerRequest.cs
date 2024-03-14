using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;
using Moq;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests : IDisposable
{
    private void TestStartServer_SetupDockerMock(StartServerRequest request, ServerInstance server)
    {
        _docker
            .Setup(
                x => x.Containers.StartContainerAsync(
                    server.ContainerId,
                    It.IsAny<ContainerStartParameters>(),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(true)
            .Verifiable();
    }

    [Fact]
    public async void TestStartServer()
    {
        var server = GetValidServerEntity();
        _context.Add(server);
        _context.SaveChanges();

        StartServerRequest request = new (_user, server.Id);
        TestStartServer_SetupDockerMock(request, server);

        var response = await _service.StartServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
    }

    [Fact]
    public async void TestStartServer_NotExists_Fails()
    {
        StartServerRequest request = new (_user, 10);

        var response = await _service.StartServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void TestStartServer_UserNotExists_Fails()
    {
        var server = GetValidServerEntity();
        _context.Add(server);
        _context.SaveChanges();

        StartServerRequest request = new (10, server.Id);

        var response = await _service.StartServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void TestStartServer_OtherOwner_Fails()
    {
        var server = GetValidServerEntity();
        var stranger = UsersServiceTests.GetValidUserEntity();
        _context.Add(server);
        _context.Add(stranger);
        _context.SaveChanges();

        StartServerRequest request = new (stranger.Id, server.Id);

        var response = await _service.StartServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
