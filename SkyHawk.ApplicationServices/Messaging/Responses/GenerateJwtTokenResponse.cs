namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class GenerateJwtTokenResponse : ResponseBase
{
    public string? BearerToken { get; set; }

    public GenerateJwtTokenResponse(BusinessStatusCodeEnum statusCode, string messageText)
        : base(statusCode, messageText)
    {

    }

    public GenerateJwtTokenResponse(string bearerToken, string messageText)
        : base(BusinessStatusCodeEnum.Success, messageText)
    {
        BearerToken = bearerToken;
    }
};
