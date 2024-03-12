using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;

namespace SkyHawk.ApplicationServices.Interfaces;

public interface IServersService
{
    // List server instances
    public Task<ListServersResponse> ListServersAsync(ListServersRequest request);

    // Get server instance
    public Task<GetServerResponse> GetServerByIdAsync(GetServerByIdRequest request);
    public Task<GetServerResponse> GetServerByNameAsync(GetServerByNameRequest request);

    // Create server instance
    public Task<CreateServerFromImageResponse> CreateServerFromImageAsync(CreateServerFromImageRequest request);

    public Task<CreateServerFromSnapshotResponse> CreateServerFromSnapshotAsync(CreateServerFromSnapshotRequest request);

    // Modify server instance
    public Task<UpdateServerResponse> UpdateServerAsync(UpdateServerRequest request);
    public Task<UpdateServerImageResponse> UpdateServerImageAsync(UpdateServerImageRequest request);
    // public Task<UpdateServerPortResponse> UpdateServerPortAsync(UpdateServerPortRequest request)

    // Delete server instance
    public Task<DeleteServerResponse> DeleteServerAsync(DeleteServerRequest request);


    // Control server instance (power on/off)
    public Task<ControlServerResponse> ControlServerAsync(ControlServerRequest request);
}
