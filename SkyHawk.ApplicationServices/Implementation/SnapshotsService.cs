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
        return new(new List<Snapshot>(), BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<GetSnapshotResponse> GetSnapshotAsync(GetByIdRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }

    public async Task<GetSnapshotResponse> GetSnapshotAsync(GetByNameRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
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
