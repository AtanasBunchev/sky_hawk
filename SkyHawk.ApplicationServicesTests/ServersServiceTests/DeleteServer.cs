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
            .Verifiable(); // Ty Pan hsttps://stackoverflow.com/questions/21253523/
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
}
