using SkyHawk.Data.Entities;
using System.IO;

namespace SkyHawk.ApplicationServices.Messaging.Requests;

public class UpdateServerImageRequest : ServerRequestBase
{
    public int ServerId { get; set; }
    public Stream Image { get; set; }

    public UpdateServerImageRequest(int userId, int serverId, Stream image) : base(userId)
    {
        ServerId = serverId;
        Image = image;
    }
};
