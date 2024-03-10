namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class AuthenticateUserResponse : ResponseBase
{
    public string? BearerToken { get; set; }

    public AuthenticateUserResponse(BusinessStatusCodeEnum statusCode, string messageText)
    {
        StatusCode = statusCode;
        MessageText = messageText;
    }

    public AuthenticateUserResponse(string bearerToken, string messageText)
    {
        BearerToken = bearerToken;
        StatusCode = BusinessStatusCodeEnum.Success;
        MessageText = messageText;
    }
};
