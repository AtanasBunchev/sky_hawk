namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetUserByNameRequest
{
    public string Name { get; set; }

    public GetUserByNameRequest(string name)
    {
        Name = name;
    }
};
