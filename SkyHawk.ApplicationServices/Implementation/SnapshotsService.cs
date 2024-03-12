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
        var snapshot = (await GetSnapshotByIdAsync(new(request.User, request.SnapshotId))).Snapshot;
        if(snapshot == null)
            return new(BusinessStatusCodeEnum.NotFound, "Snapshot not found!");

        if(request.Name != null) {
            var maxNameLength = typeof(Snapshot).GetProperty("Name")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxNameLength != null && request.Name.Length > maxNameLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Name must be less than {maxNameLength} symbols long!");
            }

            snapshot.Name = request.Name;
        }

        if(request.Description != null) {
            var maxDescriptionLength = typeof(Snapshot).GetProperty("Description")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxDescriptionLength != null && request.Description.Length > maxDescriptionLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Description must be less than {maxDescriptionLength} symbols long!");
            }

            snapshot.Description = request.Description;
        }

        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Snapshot updated successfully.");
    }


    public async Task<DeleteSnapshotResponse> DeleteSnapshotAsync(DeleteSnapshotRequest request)
    {
        var snapshot = (await GetSnapshotByIdAsync(new(request.User, request.Id))).Snapshot;
        if(snapshot == null)
            return new(BusinessStatusCodeEnum.NotFound, "Snapshot not found!");

        _context.Remove(snapshot);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Snapshot deleted successfully.");
    }

}
