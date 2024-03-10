namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class CreateUserRequest
{
    public string Username;
    public string Password;

    public CreateUserRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
};
