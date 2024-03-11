using Docker.DotNet;
using Moq;
using SkyHawk.ApplicationServices.Implementation;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.Data.Entities;
using SkyHawk.DataMock.Contexts;

namespace SkyHawk.ApplicationServicesTests;

public partial class ServersServiceTests
{
    private SkyHawkDbContextMock _context;
    private Mock<IDockerClient> _docker;
    private ISnapshotsService _service;

    private User _user;

    public ServersServiceTests()
    {
        _context = new();
        _docker = new();
        _service = new SnapshotsService(_context, _docker.Object);

        _user = UsersServiceTests.GetValidUserEntity();
        _context.Add(_user);
        _context.SaveChanges();
    }

    public ServerInstance GetValidServerEntity()
    {
        return new (){
            ContainerId = new String('0', 64),
            Owner = _user,
            Name = "My Server",
            Description = "I has a game server :D"
        };
    }
}
