using SkyHawk.Data.Entities;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GetServerResponse : ResponseBase
{
    public ServerInstance? Server { get; set; }

    public GetServerResponse(ServerInstance? server)
        : base(BusinessStatusCodeEnum.None)
    {
        Server = server;
        if (server == null) {
            StatusCode = BusinessStatusCodeEnum.NotFound;
            MessageText = "Server not found!";
        } else {
            StatusCode = BusinessStatusCodeEnum.Success;
            MessageText = "Server fetched successfully.";
        }
    }
};
