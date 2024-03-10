using Docker.DotNet;
using Moq;
using SkyHawk.ApplicationServices.Implementation;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.Data.Entities;
using SkyHawk.DataMock.Contexts;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    private SkyHawkDbContextMock _context;
    private Mock<IDockerClient> _docker;
    private IUsersService _service;

    public UsersServiceTests()
    {
        _context = new();
        _docker = new();
        _service = new UsersService(_context, _docker.Object);
    }

    public User GetValidUserEntity()
    {
        return new (){
            Username = "Name",
            Password = "Pass",
        };
    }
}
