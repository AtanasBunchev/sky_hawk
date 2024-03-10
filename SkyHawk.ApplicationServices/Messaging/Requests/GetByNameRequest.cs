namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetByNameRequest
{
    public string Name { get; set; }

    public GetByNameRequest(string name)
    {
        Name = name;
    }
};
