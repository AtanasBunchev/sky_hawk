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
    private IServersService _service;

    private int _user;
    private User _userObject;

    public ServersServiceTests()
    {
        _context = new();
        _docker = new(MockBehavior.Strict);
        _service = new ServersService(_context, _docker.Object);

        _userObject = UsersServiceTests.GetValidUserEntity();
        _context.Add(_userObject);
        _context.SaveChanges();
        _user = _userObject.Id;
    }

    public ServerInstance GetValidServerEntity()
    {
        return new (){
            ContainerId = new String('0', 64),
            Owner = _userObject,
            Name = "My Server",
            Description = "I has a game server :D"
        };
    }
}
