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
    private void TestStopServer_SetupDockerMock(StopServerRequest request, ServerInstance server)
    {
        _docker
            .Setup(
                x => x.Containers.StopContainerAsync(
                    server.ContainerId,
                    It.IsAny<ContainerStopParameters>(),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(true)
            .Verifiable();
    }

    [Fact]
    public async void TestStopServer()
    {
        var server = GetValidServerEntity();
        _context.Add(server);
        _context.SaveChanges();

        StopServerRequest request = new (_user, server.Id);
        TestStopServer_SetupDockerMock(request, server);

        var response = await _service.StopServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
    }

    [Fact]
    public async void TestStopServer_NotExists_Fails()
    {
        StopServerRequest request = new (_user, 10);

        var response = await _service.StopServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void TestStopServer_UserNotExists_Fails()
    {
        var server = GetValidServerEntity();
        _context.Add(server);
        _context.SaveChanges();

        StopServerRequest request = new (10, server.Id);

        var response = await _service.StopServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void TestStopServer_OtherOwner_Fails()
    {
        var server = GetValidServerEntity();
        var stranger = UsersServiceTests.GetValidUserEntity();
        _context.Add(server);
        _context.Add(stranger);
        _context.SaveChanges();

        StopServerRequest request = new (stranger.Id, server.Id);

        var response = await _service.StopServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
