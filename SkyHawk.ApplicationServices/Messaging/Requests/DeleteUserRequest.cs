namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class DeleteUserRequest
{
    public int Id { get; set; }

    public DeleteUserRequest(int id)
    {
        Id = id;
    }
};
