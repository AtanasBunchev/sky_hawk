namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class CreateServerFromImageResponse : ResponseBase
{
    public int ServerId { get; set; } = -1;

    public CreateServerFromImageResponse(BusinessStatusCodeEnum statusCode, String? messageText = null)
        : base(statusCode, messageText)
    {

    }

    public CreateServerFromImageResponse(int serverId)
        : base(BusinessStatusCodeEnum.Success, "Server created successfully.")
    {
        ServerId = serverId;
    }
};
