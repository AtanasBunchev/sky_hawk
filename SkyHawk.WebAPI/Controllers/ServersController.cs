using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.Data.Entities;
using SkyHawk.Data.Server;
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


    public class CreateServerFromImageModel
    {
        public ServerType Type { get; set; }
        public int Port { get; set; }
        public string Name { get; set; } = null!; // suppress warnings
        public string Description { get; set; } = null!;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateServerFromImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateServerFromImageResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateServerFromImageResponse>> CreateServerFromImageAsync([FromBody] CreateServerFromImageModel request)
    {
        CreateServerFromImageRequest serviceRequest = new(_user, request.Type, request.Port)
        {
            Name = request.Name,
            Description = request.Description
        };

        var response = await _service.CreateServerFromImageAsync(serviceRequest);
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }


    public class CreateServerFromSnapshotModel
    {
        public int Port { get; set; }
        public string Name { get; set; } = null!; // suppress warnings
        public string Description { get; set; } = null!;
    }

    [HttpPost("from_snapshot/{snapshotId}")]
    [ProducesResponseType(typeof(CreateServerFromSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateServerFromSnapshotResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateServerFromSnapshotResponse>> CreateServerFromSnapshotAsync([FromRoute] int snapshotId, [FromBody] CreateServerFromSnapshotModel request)
    {
        var response = await _service.CreateServerFromSnapshotAsync(new(_user, snapshotId, request.Port)
        {
            Name = request.Name,
            Description = request.Description
        });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }


    public class ServerUpdateTuple
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TimeOnly? AutoStart { get; set; }
        public TimeOnly? AutoStop { get; set; }
    }

    [HttpPut("{id}")]
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(UpdateServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateServerResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateServerResponse>> UpdateServerAsync([FromRoute] int id, [FromBody] ServerUpdateTuple tuple)
    {
        var response = await _service.UpdateServerAsync(new (_user, id)
        {
            Name = tuple.Name,
            Description = tuple.Description,
            AutoStart = tuple.AutoStart,
            AutoStop = tuple.AutoStop
        });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpPut("{id}/name")]
    [ProducesResponseType(typeof(UpdateServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateServerResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateServerResponse>> UpdateServerNameAsync([FromRoute] int id, [FromBody] string name)
    {
        var response = await _service.UpdateServerAsync(new (_user, id) { Name = name });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpPut("{id}/description")]
    [ProducesResponseType(typeof(UpdateServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateServerResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateServerResponse>> UpdateServerDescriptionAsync([FromRoute] int id, [FromBody] string description)
    {
        var response = await _service.UpdateServerAsync(new (_user, id) { Description = description });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    // TODO StartTime and EndTime


    [HttpPut("{id}/image")]
    [ProducesResponseType(typeof(UpdateServerImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateServerImageResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateServerImageResponse>> UpdateServerAsync([FromRoute] int id, [FromForm] IFormFile image)
    {
        if(image == null || image.Length == 0)
            return BadRequest(new UpdateServerImageResponse(BusinessStatusCodeEnum.InvalidInput, "Please provide a PNG image!"));

        string ext = Path.GetExtension(image.FileName);
        if(ext != ".png" && ext != ".PNG")
            return BadRequest(new UpdateServerImageResponse(BusinessStatusCodeEnum.InvalidInput, "Please provide a PNG image!"));

        var response = await _service.UpdateServerImageAsync(new (_user, id, image.OpenReadStream()));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DeleteServerResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteServerResponse>> DeleteServerAsync([FromRoute] int id)
    {
        var response = await _service.DeleteServerAsync(new (_user, id));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpPost("{id}/power/on")]
    [ProducesResponseType(typeof(StartServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StartServerResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StartServerResponse>> StartServerAsync([FromRoute] int id)
    {
        var response = await _service.StartServerAsync(new (_user, id));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpPost("{id}/power/off")]
    [ProducesResponseType(typeof(StopServerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StopServerResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StopServerResponse>> StopServerAsync([FromRoute] int id)
    {
        var response = await _service.StopServerAsync(new (_user, id));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }
}
