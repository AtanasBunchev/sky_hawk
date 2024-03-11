namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class AuthenticateUserResponse : ResponseBase
{
    public string? BearerToken { get; set; }

    public AuthenticateUserResponse(BusinessStatusCodeEnum statusCode, string messageText)
        : base(statusCode, messageText)
    {

    }

    public AuthenticateUserResponse(string bearerToken, string messageText)
        : base(BusinessStatusCodeEnum.Success, messageText)
    {
        BearerToken = bearerToken;
    }
};
