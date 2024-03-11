namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetUserByIdRequest
{
    public int Id { get; set; }

    public GetUserByIdRequest(int id)
    {
        Id = id;
    }
};
