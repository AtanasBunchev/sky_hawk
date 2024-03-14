using Docker.DotNet;
using Moq;
using SkyHawk.ApplicationServices.Implementation;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.Data.Entities;
using SkyHawk.DataMock.Contexts;

namespace SkyHawk.ApplicationServicesTests;

public partial class SnapshotsServiceTests : IDisposable
{
    private SkyHawkDbContextMock _context;
    private Mock<IDockerClient> _docker;
    private ISnapshotsService _service;

    private int _user;
    private User _userObject;

    public SnapshotsServiceTests()
    {
        _context = new();
        _docker = new();
        _service = new SnapshotsService(_context, _docker.Object);

        _userObject = UsersServiceTests.GetValidUserEntity();
        _context.Add(_userObject);
        _context.SaveChanges();
        _user = _userObject.Id;
    }

    public Snapshot GetValidSnapshotEntity()
    {
        return new (){
            ImageId = new String('0', 64),
            Owner = _userObject,
            Name = "My Snapshot",
            Description = "I has a snapshot :D"
        };
    }

    public void Dispose()
    {
        _docker.Verify();
    }
}
