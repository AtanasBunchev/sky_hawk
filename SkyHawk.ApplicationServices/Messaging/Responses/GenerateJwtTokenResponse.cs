namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GenerateJwtTokenResponse : ResponseBase
{
    public string? BearerToken { get; set; }

    public GenerateJwtTokenResponse(BusinessStatusCodeEnum statusCode, string messageText)
    {
        StatusCode = statusCode;
        MessageText = messageText;
    }

    public GenerateJwtTokenResponse(string bearerToken, string messageText)
    {
        BearerToken = bearerToken;
        StatusCode = BusinessStatusCodeEnum.Success;
        MessageText = messageText;
    }
};
