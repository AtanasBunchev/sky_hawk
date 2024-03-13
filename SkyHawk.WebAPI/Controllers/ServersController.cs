using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace SkyHawk.WebAPI.Controllers;

[Route("servers")]
[ApiController]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ServersController : ControllerBase
{
    private IServersService _service;
    private int _user = -1; // invalid id

    public ServersController(IServersService service)
    {
        _service = service;

        var claim = HttpContext?.User?.Claims?.SingleOrDefault(u => u.Type == "LoggedUserId");
        int.TryParse(claim?.Value, out _user);
    }

    [HttpGet]
    [HttpGet("list")]
    [ProducesResponseType(typeof(ListServersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListServersResponse>> ListServers()
    {
        return Ok(await _service.ListServersAsync(new(_user)));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetServerResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetServerResponse>> GetServerByIdAsync(int id)
    {
        GetServerResponse response = await _service.GetServerByIdAsync(new(_user, id));
        if(response.Server == null)
            return NotFound(response);
        return Ok(response);
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(GetServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetServerResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetServerResponse>> GetServerByNameAsync(string name)
    {
        var response = await _service.GetServerByNameAsync(new(_user, name));
        if(response.Server == null)
            return NotFound(response);
        return Ok(response);
    }


    [HttpPost]
    [ProducesResponseType(typeof(CreateServerFromImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateServerFromImageResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateServerFromImageResponse>> CreateServerFromImageAsync([FromBody] CreateServerFromImageRequest request)
    {
        var response = await _service.CreateServerFromImageAsync(request);
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

}
