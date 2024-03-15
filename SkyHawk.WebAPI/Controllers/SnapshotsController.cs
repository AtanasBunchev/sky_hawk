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

[Route("snapshots")]
[ApiController]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SnapshotsController : ControllerBase
{
    private ISnapshotsService _service;
    private int _user { get {
        var claim = HttpContext?.User?.Claims?.SingleOrDefault(u => u.Type == "LoggedUserId");
        int id = -1;
        int.TryParse(claim?.Value, out id);
        return id;
    } }

    public SnapshotsController(ISnapshotsService service)
    {
        _service = service;
    }

    [HttpGet]
    [HttpGet("list")]
    [ProducesResponseType(typeof(ListSnapshotsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListSnapshotsResponse>> ListSnapshots([FromQuery] int page = 1, [FromQuery] int pageSize = -1)
    {
        return Ok(await _service.ListSnapshotsAsync(new(_user, page, pageSize)));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetSnapshotResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetSnapshotResponse>> GetSnapshotByIdAsync(int id)
    {
        GetSnapshotResponse response = await _service.GetSnapshotByIdAsync(new(_user, id));
        if(response.Snapshot == null)
            return NotFound(response);
        return Ok(response);
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(GetSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetSnapshotResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetSnapshotResponse>> GetSnapshotByNameAsync(string name)
    {
        var response = await _service.GetSnapshotByNameAsync(new(_user, name));
        if(response.Snapshot == null)
            return NotFound(response);
        return Ok(response);
    }


    public class CreateSnapshotModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    [HttpPost("from_server/{serverId}")]
    [ProducesResponseType(typeof(CreateSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateSnapshotResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateSnapshotResponse>> CreateSnapshotAsync([FromRoute] int serverId, [FromBody] CreateSnapshotModel request)
    {
        var response = await _service.CreateSnapshotAsync(new(_user, serverId)
        {
            Name = request.Name,
            Description = request.Description
        });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }


    public class SnapshotUpdateTuple
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    [HttpPut("{id}")]
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(UpdateSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateSnapshotResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateSnapshotResponse>> UpdateSnapshotAsync([FromRoute] int id, [FromBody] SnapshotUpdateTuple tuple)
    {
        var response = await _service.UpdateSnapshotAsync(new (_user, id)
        {
            Name = tuple.Name,
            Description = tuple.Description
        });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpPut("{id}/name")]
    [ProducesResponseType(typeof(UpdateSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateSnapshotResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateSnapshotResponse>> UpdateSnapshotNameAsync([FromRoute] int id, [FromBody] string name)
    {
        var response = await _service.UpdateSnapshotAsync(new (_user, id) { Name = name });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpPut("{id}/description")]
    [ProducesResponseType(typeof(UpdateSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateSnapshotResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateSnapshotResponse>> UpdateSnapshotDescriptionAsync([FromRoute] int id, [FromBody] string description)
    {
        var response = await _service.UpdateSnapshotAsync(new (_user, id) { Description = description });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteSnapshotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DeleteSnapshotResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteSnapshotResponse>> DeleteSnapshotAsync([FromRoute] int id)
    {
        var response = await _service.DeleteSnapshotAsync(new (_user, id));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }
}
