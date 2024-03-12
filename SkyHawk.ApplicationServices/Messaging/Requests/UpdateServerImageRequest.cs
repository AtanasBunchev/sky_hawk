using SkyHawk.Data.Entities;
using System.IO;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateServerImageRequest : ServerRequestBase
{
    public int ServerId { get; set; }
    public Stream Image { get; set; }

    public UpdateServerImageRequest(User user, int serverId, Stream image) : base(user)
    {
        ServerId = serverId;
        Image = image;
    }
};
