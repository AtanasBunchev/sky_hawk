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

[Route("users")]
[ApiController]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private IUsersService _service;

    public UserController(IUsersService service)
    {
        _service = service;
    }

    public class UsernamePasswordTuple
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    [HttpGet]
    [HttpGet("list")]
    [ProducesResponseType(typeof(ListUsersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListUsersResponse>> ListUsers([FromQuery] int page = 1, [FromQuery] int pageSize = -1)
    {
        return Ok(await _service.ListUsersAsync(new(page, pageSize)));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetUserResponse>> GetUserByIdAsync(int id)
    {
        GetUserResponse response = await _service.GetUserByIdAsync(new(id));
        if(response.User == null)
            return NotFound(response);
        return Ok(response);
    }

    [HttpGet("username/{name}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetUserResponse>> GetUserByNameAsync(string name)
    {
        var response = await _service.GetUserByNameAsync(new(name));
        if(response.User == null)
            return NotFound(response);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateUserResponse>> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var response = await _service.CreateUserAsync(request);
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }

    [HttpPut]
    [HttpPatch]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUserAsync([FromBody] UsernamePasswordTuple tuple)
    {
        var claim = HttpContext?.User?.Claims?.SingleOrDefault(u => u.Type == "LoggedUserId");
        if (!int.TryParse(claim?.Value, out int id))
            return BadRequest(new UpdateUserResponse(BusinessStatusCodeEnum.InvalidInput, "Invalid Id"));

        var response = await _service.UpdateUserAsync(new(id, tuple.Username, tuple.Password));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpPut("username")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserResponse>> PatchUserNameAsync([FromBody] string username)
    {
        var claim = HttpContext?.User?.Claims?.SingleOrDefault(u => u.Type == "LoggedUserId");
        if (!int.TryParse(claim?.Value, out int id))
            return BadRequest(new UpdateUserResponse(BusinessStatusCodeEnum.InvalidInput, "Invalid Id"));

        var response = await _service.UpdateUserAsync(new(id) { Username = username });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpPut("password")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserResponse>> PatchUserPasswordAsync([FromBody] string password)
    {
        var claim = HttpContext?.User?.Claims?.SingleOrDefault(u => u.Type == "LoggedUserId");
        if (!int.TryParse(claim?.Value, out int id))
            return BadRequest(new UpdateUserResponse(BusinessStatusCodeEnum.InvalidInput, "Invalid Id"));

        var response = await _service.UpdateUserAsync(new(id) { Password = password });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpDelete("delete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteUserResponse>> DeleteUserAsync()
    {
        var claim = HttpContext?.User?.Claims?.SingleOrDefault(u => u.Type == "LoggedUserId");
        if (!int.TryParse(claim?.Value, out int id))
            return BadRequest(new DeleteUserResponse(BusinessStatusCodeEnum.InvalidInput, "Invalid Id"));

        var response = await _service.DeleteUserAsync(new(id));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);

        return BadRequest(response);
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthenticateUserResponse>> AuthenticateUserAsync([FromBody] AuthenticateUserRequest request)
    {
        var result = await _service.AuthenticateUserAsync(request);
        if(result.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(result);

        if(result.StatusCode != BusinessStatusCodeEnum.Success)
            return BadRequest(result);

        var header = new AuthenticationHeaderValue("Bearer", result.BearerToken);
        Response.Headers.Authorization = header.ToString();
        return Ok(result);
    }

}
