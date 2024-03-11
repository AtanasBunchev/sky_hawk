using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;

namespace SkyHawk.ApplicationServices.Interfaces;

public interface IServersService
{
    // List server instances
    public Task<ListServersResponse> ListServersAsync(ListServersRequest request);

    // Get server instance
    public Task<GetServerResponse> GetServerByIdAsync(GetByIdRequest request);
    public Task<GetServerResponse> GetServerByNameAsync(GetByNameRequest request);

    // Create server instance
    public Task<CreateServerFromImageResponse> CreateServerFromImageAsync(CreateServerFromImageRequest request);

    public Task<CreateServerFromSnapshotResponse> CreateServerFromSnapshotAsync(CreateServerFromSnapshotRequest request);

    // Modify server instance
    public Task<UpdateServerResponse> UpdateServerAsync(UpdateServerRequest request);

    // Delete server instance
    public Task<DeleteServerResponse> DeleteServerAsync(DeleteServerRequest request);


    // Control server instance (power on/off)
    public Task<ControlServerResponse> ControlServerAsync(ControlServerRequest request);
}
