using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyHawk.Data.Entities;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.ApplicationServices.Messaging.Responses;
using SkyHawk.ApplicationServices.Interfaces;

namespace SkyHawk.WebAPI.Controllers;

[Route("users")]
[ApiController]
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
    public async Task<ActionResult<ListUsersResponse>> GetUsers()
    {
        return Ok(await _service.ListUsersAsync(new()));
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

    [HttpGet("{name}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserByNameAsync(string name)
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

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUserAsync(int id, [FromBody] UsernamePasswordTuple tuple)
    {
        var response = await _service.UpdateUserAsync(new(id, tuple.Username, tuple.Password));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpPatch("{id}/username/{username}")]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserResponse>> PatchUserNameAsync(int id, string username)
    {
        var response = await _service.UpdateUserAsync(new(id) { Username = username });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpPatch("{id}/password/{password}")]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserResponse>> PatchUserPasswordAsync(int id, string password)
    {
        var response = await _service.UpdateUserAsync(new(id) { Password = password });
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteUserResponse>> DeleteUserAsync(int id)
    {
        var response = await _service.UpdateUserAsync(new(id));
        if(response.StatusCode == BusinessStatusCodeEnum.Success)
            return Ok(response);
        else if(response.StatusCode == BusinessStatusCodeEnum.NotFound)
            return NotFound(response);

        return BadRequest(response);
    }
}
