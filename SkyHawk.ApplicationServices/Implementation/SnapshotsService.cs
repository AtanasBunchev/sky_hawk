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

namespace SkyHawk.ApplicationServices.Implementation;

public class SnapshotsService : ISnapshotsService
{
    SkyHawkDbContext _context;
    IDockerClient _docker;

    public SnapshotsService(SkyHawkDbContext context, IDockerClient docker)
    {
        _context = context;
        _docker = docker;
    }

    public async Task<ListSnapshotsResponse> ListSnapshotsAsync(ListSnapshotsRequest request)
    {
        _context.Snapshots
            .Include(c => c.Owner)
            .Load();

        return new(await _context.Snapshots.Where(x => x.Owner.Id == request.User.Id).ToListAsync());
    }


    public async Task<GetSnapshotResponse> GetSnapshotByIdAsync(GetSnapshotByIdRequest request)
    {
        _context.Snapshots
            .Include(c => c.Owner)
            .Load();

        return new(
            await _context.Snapshots
                .SingleOrDefaultAsync(x => 
                    x.Owner.Id == request.User.Id &&
                    x.Id == request.Id)
        );
    }

    public async Task<GetSnapshotResponse> GetSnapshotByNameAsync(GetSnapshotByNameRequest request)
    {
        _context.Snapshots
            .Include(c => c.Owner)
            .Load();

        return new(
            await _context.Snapshots
                .SingleOrDefaultAsync(x => 
                    x.Owner.Id == request.User.Id &&
                    x.Name == request.Name)
        );
    }


    public async Task<CreateSnapshotResponse> CreateSnapshotAsync(CreateSnapshotRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<UpdateSnapshotResponse> UpdateSnapshotAsync(UpdateSnapshotRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<DeleteSnapshotResponse> DeleteSnapshotAsync(DeleteSnapshotRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }

}
