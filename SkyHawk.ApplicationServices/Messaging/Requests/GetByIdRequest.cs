namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class GetByIdRequest
{
    public int Id { get; set; }

    public GetByIdRequest(int id)
    {
        Id = id;
    }
};
