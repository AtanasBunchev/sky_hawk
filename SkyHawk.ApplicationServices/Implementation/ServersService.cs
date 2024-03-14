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

        return new(await _context.Servers.Where(x => x.Owner.Id == request.UserId).ToListAsync());
    }


    public async Task<GetServerResponse> GetServerByIdAsync(GetServerByIdRequest request)
    {
        _context.Servers
            .Include(c => c.Owner)
            .Load();

        return new(
            await _context.Servers
                .SingleOrDefaultAsync(x => 
                    x.Owner.Id == request.UserId &&
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
                    x.Owner.Id == request.UserId &&
                    x.Name == request.Name)
        );
    }


    public async Task<CreateServerFromImageResponse> CreateServerFromImageAsync(CreateServerFromImageRequest request)
    {
        string? errorMessage = CreateServer_ValidateArguments(request.Type, request.Port, request.Name, request.Description);
        var data = ServerDefaults.Get(request.Type);
        if(errorMessage != null || data == null) // Cover a false warning
            return new(BusinessStatusCodeEnum.InvalidInput, errorMessage);

        var user = _context.Users.SingleOrDefault(x => x.Id == request.UserId);
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");

        string image = await CreateServer_DownloadDockerImageAsync(data.Image, data.Tag);
        string containerId = await CreateServer_CreateContainerAsync(request.Type, image, request.Port);

        ServerInstance server = new ServerInstance {
            ContainerId = containerId,
            Type = request.Type,
            Port = request.Port,

            Owner = user,

            Name = request.Name,
            Description = request.Description
        };
        _context.Servers.Add(server);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success);
    }

    public async Task<CreateServerFromSnapshotResponse> CreateServerFromSnapshotAsync(CreateServerFromSnapshotRequest request)
    {
        var snapshot = await _context.Snapshots.SingleOrDefaultAsync
            (x => x.Owner.Id == request.UserId && x.Id == request.SnapshotId);
        if(snapshot == null)
            return new(BusinessStatusCodeEnum.NotFound, "Snapshot not found!");

        string? errorMessage = CreateServer_ValidateArguments(snapshot.Type, request.Port, request.Name, request.Description);
        if(errorMessage != null)
            return new(BusinessStatusCodeEnum.InvalidInput, errorMessage);

        var user = _context.Users.SingleOrDefault(x => x.Id == request.UserId);
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");

        var data = ServerDefaults.Get(snapshot.Type);
        string containerId = await CreateServer_CreateContainerAsync(snapshot.Type, snapshot.ImageId, request.Port);

        // Store the server data
        ServerInstance server = new ServerInstance {
            ContainerId = containerId,
            Type = snapshot.Type,
            Port = request.Port,

            Owner = user,

            Name = request.Name,
            Description = request.Description
        };

        _context.Servers.Add(server);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success);
    }

    /// <returns> Error message or null on success </returns>
    private string? CreateServer_ValidateArguments(ServerType type, int port, string name, string description)
    {
        var data = ServerDefaults.Get(type);
        if(data == null)
            return "Unknown ServerType defaults!";

        if(port <= 1000)
            return "Port must be over 1000!";

        if(name != null) {
            var maxNameLength = typeof(ServerInstance).GetProperty("Name")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxNameLength != null && name.Length > maxNameLength)
                return $"Name must be less than {maxNameLength} symbols long!";
        }

        if(description != null) {
            var maxDescriptionLength = typeof(ServerInstance).GetProperty("Description")
                    ?.GetCustomAttribute<MaxLengthAttribute>()?.Length;
            if(maxDescriptionLength != null && description.Length > maxDescriptionLength)
                return $"Description must be less than {maxDescriptionLength} symbols long!";
        }

        return null;
    }

    /// <returns> Image Name and Tag </returns>
    private async Task<string> CreateServer_DownloadDockerImageAsync(string image, string tag)
    {
        await _docker.Images.CreateImageAsync(
            new ImagesCreateParameters
            {
                FromImage = image,
                Tag = tag,
            },
            null,
            new Progress<JSONMessage>());

        return $"{image}:{tag}";
    }

    /// <returns> Container ID </returns>
    private async Task<string> CreateServer_CreateContainerAsync(ServerType type, string image, int port)
    {
        var data = ServerDefaults.Get(type);
        string protocol = data.Protocol == PortProtocol.UDP ? "udp" : "tcp";

        CreateContainerResponse response = await _docker.Containers.CreateContainerAsync(new CreateContainerParameters()
        {
            Image = image,
            HostConfig = new HostConfig()
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    [$"{data.InternalPort}/{protocol}"] = new List<PortBinding>
                    {
                        new PortBinding {
                            HostPort = $"{port}"
                        }
                    }
                },
                DNS = new[] { "9.9.9.9", "1.1.1.1", "8.8.8.8", "8.8.4.4" }
            },
            Env = data.Env
        });

        return response.ID;
    }


    public async Task<UpdateServerResponse> UpdateServerAsync(UpdateServerRequest request)
    {
        var server = (await GetServerByIdAsync(new(request.UserId, request.ServerId))).Server;
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
            var result = await UpdateServerImageAsync(new(request.UserId, request.ServerId, request.Image));
            if(result.StatusCode != BusinessStatusCodeEnum.Success)
                return new(result.StatusCode, result.MessageText);
        }

        /*
        if(request.Port != null) {
            var result = await UpdateServerPortAsync(new(request.UserId, request.ServerId, request.Port));
            if(result.StatusCode != BusinessStatusCodeEnum.Success)
                return new(result.StatusCode, result.MessageText);
        }
        */

        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Server updated successfully.");
    }

    public async Task<UpdateServerImageResponse> UpdateServerImageAsync(UpdateServerImageRequest request)
    {
        var server = (await GetServerByIdAsync(new(request.UserId, request.ServerId))).Server;
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
        var server = (await GetServerByIdAsync(new(request.UserId, request.Id))).Server;
        if(server == null)
            return new(BusinessStatusCodeEnum.NotFound, "Server not found!");

        _docker.Containers.RemoveContainerAsync(server.ContainerId, null);

        _context.Remove(server);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Server deleted successfully.");
    }


    public async Task<ControlServerResponse> ControlServerAsync(ControlServerRequest request)
    {
        return new(BusinessStatusCodeEnum.NotImplemented);
    }
}
