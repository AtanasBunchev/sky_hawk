using Docker.DotNet;
using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Contexts;
using SkyHawk.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;

namespace SkyHawk.ApplicationServices.Implementation;

public class ServersService : IServersService
{
    SkyHawkDbContext _context;
    IDockerClient _docker;

    public ServersService(SkyHawkDbContext context, IDockerClient docker)
    {
        _context = context;
        _docker = docker;
    }

    public async Task<ListServersResponse> ListServersAsync(ListServersRequest request)
    {
        _context.Servers
            .Include(c => c.Owner)
            .Load();

        return new(await _context.Servers.Where(x => x.Owner.Id == request.User.Id).ToListAsync());
    }


    public async Task<GetServerResponse> GetServerByIdAsync(GetServerByIdRequest request)
    {
        _context.Servers
            .Include(c => c.Owner)
            .Load();

        return new(
            await _context.Servers
                .SingleOrDefaultAsync(x => 
                    x.Owner.Id == request.User.Id &&
                    x.Id == request.Id)
        );
    }

    public async Task<GetServerResponse> GetServerByNameAsync(GetServerByNameRequest request)
    {
        _context.Servers
            .Include(c => c.Owner)
            .Load();

        return new(
            await _context.Servers
                .SingleOrDefaultAsync(x => 
                    x.Owner.Id == request.User.Id &&
                    x.Name == request.Name)
        );
    }


    public async Task<CreateServerFromImageResponse> CreateServerFromImageAsync(CreateServerFromImageRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<CreateServerFromSnapshotResponse> CreateServerFromSnapshotAsync(CreateServerFromSnapshotRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<UpdateServerResponse> UpdateServerAsync(UpdateServerRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<DeleteServerResponse> DeleteServerAsync(DeleteServerRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<ControlServerResponse> ControlServerAsync(ControlServerRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }
}
