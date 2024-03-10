namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class AuthenticateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }

    // Optionally add more device-specific information such as User Agent and IP

    public AuthenticateUserRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
};
