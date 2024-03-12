using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Contexts;
using SkyHawk.Data.Entities;
using SkyHawk.Data.Server;
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
        var defaults = ServerDefaults.Get(request.Type);
        if(defaults == null)
            return new(BusinessStatusCodeEnum.InvalidInput, "Unknown ServerType defaults!");

        if(request.Port <= 1000)
            return new(BusinessStatusCodeEnum.InvalidInput, "Port must be over 1000!");

        if(request.Name != null) {
            var maxNameLength = typeof(ServerInstance).GetProperty("Name")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxNameLength != null && request.Name.Length > maxNameLength)
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Name must be less than {maxNameLength} symbols long!");
        }

        if(request.Description != null) {
            var maxDescriptionLength = typeof(ServerInstance).GetProperty("Description")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxDescriptionLength != null && request.Description.Length > maxDescriptionLength)
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Description must be less than {maxDescriptionLength} symbols long!");
        }


        // Download the image
        await _docker.Images.CreateImageAsync(
            new ImagesCreateParameters
            {
                FromImage = defaults.Image,
                Tag = defaults.Tag,
            },
            null,
            new Progress<JSONMessage>());

        string protocol = defaults.Protocol == PortProtocol.UDP ? "udp" : "tcp";

        Dictionary<string, EmptyStruct> exposedPorts = new();
        exposedPorts.Add($"{request.Port}:{defaults.InternalPort}/{protocol}", default);

        CreateContainerResponse response = await _docker.Containers.CreateContainerAsync(new CreateContainerParameters()
        {
            Image = $"{defaults.Image}:{defaults.Tag}",
            HostConfig = new HostConfig()
            {
                DNS = new[] { "9.9.9.9", "8.8.8.8", "8.8.4.4", "1.1.1.1" }
            },
            Env = defaults.Env,
            ExposedPorts = exposedPorts
        });

        string containerId = response.ID;

        ServerInstance server = new ServerInstance {
            ContainerId = containerId,
            Type = request.Type,
            Port = request.Port,

            Owner = request.User,

            Name = request.Name,
            Description = request.Description
        };

        _context.Servers.Add(server);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success);
    }


    public async Task<CreateServerFromSnapshotResponse> CreateServerFromSnapshotAsync(CreateServerFromSnapshotRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }


    public async Task<UpdateServerResponse> UpdateServerAsync(UpdateServerRequest request)
    {
        var server = (await GetServerByIdAsync(new(request.User, request.ServerId))).Server;
        if(server == null)
            return new(BusinessStatusCodeEnum.NotFound, "Server not found!");

        if(request.Name != null) {
            var maxNameLength = typeof(ServerInstance).GetProperty("Name")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxNameLength != null && request.Name.Length > maxNameLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Name must be less than {maxNameLength} symbols long!");
            }

            server.Name = request.Name;
        }

        if(request.Description != null) {
            var maxDescriptionLength = typeof(ServerInstance).GetProperty("Description")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxDescriptionLength != null && request.Description.Length > maxDescriptionLength) {
                return new(BusinessStatusCodeEnum.InvalidInput,
                        $"Description must be less than {maxDescriptionLength} symbols long!");
            }

            server.Description = request.Description;
        }

        if(request.AutoStart != null)
            server.AutoStart = request.AutoStart;

        if(request.AutoStop != null)
            server.AutoStop = request.AutoStop;

        if(request.AutoStart != null && request.AutoStart == request.AutoStop) {
            server.AutoStart = null;
            server.AutoStop = null;
        }

        if(request.Image != null) {
            var result = await UpdateServerImageAsync(new(request.User, request.ServerId, request.Image));
            if(result.StatusCode != BusinessStatusCodeEnum.Success)
                return new(result.StatusCode, result.MessageText);
        }

        /*
        if(request.Port != null) {
            var result = await UpdateServerPortAsync(new(request.User, request.ServerId, request.Port));
            if(result.StatusCode != BusinessStatusCodeEnum.Success)
                return new(result.StatusCode, result.MessageText);
        }
        */

        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Server updated successfully.");
    }

    public async Task<UpdateServerImageResponse> UpdateServerImageAsync(UpdateServerImageRequest request)
    {
        var server = (await GetServerByIdAsync(new(request.User, request.ServerId))).Server;
        if(server == null)
            return new(BusinessStatusCodeEnum.NotFound, "Server not found!");

        server.Image = new byte[request.Image.Length];

        request.Image.ReadExactly(server.Image, 0, server.Image.Length);

        _context.SaveChanges();

        return new(BusinessStatusCodeEnum.Success);
    }

    /*
    public async Task<UpdateServerPortResponse> UpdateServerPortAsync(UpdateServerPortRequest request)
    {
        // TODO store a container as image, destroy it, recreate it, start it, remove the image
        return new(BusinessStatusCodeEnum.NotImplemented);
    }
    */


    public async Task<DeleteServerResponse> DeleteServerAsync(DeleteServerRequest request)
    {
        var server = (await GetServerByIdAsync(new(request.User, request.Id))).Server;
        if(server == null)
            return new(BusinessStatusCodeEnum.NotFound, "Server not found!");

        _context.Remove(server);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Server deleted successfully.");
    }


    public async Task<ControlServerResponse> ControlServerAsync(ControlServerRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }
}
