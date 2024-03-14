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
    private void TestDeleteServer_SetupDockerMock(DeleteServerRequest request, ServerInstance server)
    {
        _docker
            .Setup(
                x => x.Containers.RemoveContainerAsync(
                    server.ContainerId,
                    It.IsAny<ContainerRemoveParameters>(),
                    It.IsAny<CancellationToken>()
                ))
            .Returns(Task.FromResult(default(object)))
            .Verifiable();
    }


    [Fact]
    public async void TestDeleteServer()
    {
        var server = GetValidServerEntity();
        _context.Add(server);
        _context.SaveChanges();
        int id = server.Id;

        DeleteServerRequest request = new(_user, id);
        TestDeleteServer_SetupDockerMock(request, server);

        var response = await _service.DeleteServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        Assert.Empty(_context.Servers.ToList());

        response = await _service.DeleteServerAsync(new(_user, id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }


    [Fact]
    public async void TestDeleteSnapshot_OtherOwner_Fails()
    {
        var server = GetValidServerEntity();
        var stranger = UsersServiceTests.GetValidUserEntity();
        _context.Add(server);
        _context.Add(stranger);
        _context.SaveChanges();

        DeleteServerRequest request = new (stranger.Id, server.Id);
        var response = await _service.DeleteServerAsync(request);
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);

        Assert.Single(_context.Servers.ToList());
    }
}
