using SkyHawk.Data.Entities;
using System.Collections.Generic;

namespace SkyHawk.ApplicationServices.Messaging.Responses;

public class ListServersResponse : ResponseBase
{
    public ICollection<ServerInstance> Servers { get; set; }

    public ListServersResponse(ICollection<ServerInstance> servers)
        : base(BusinessStatusCodeEnum.Success, "Servers list fetched successfully.")
    {
        Servers = servers;
    }

    public ListServersResponse(ICollection<ServerInstance> servers, BusinessStatusCodeEnum statusCode)
        : base(statusCode)
    {
        Servers = servers;
    }

    public ListServersResponse(ICollection<ServerInstance> servers, BusinessStatusCodeEnum statusCode, string msg)
        : base(statusCode, msg)
    {
        Servers = servers;
    }
};
