using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Models.RequestModels;
using UserService.Models.ResponseModels;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
    {
        var response = await _mediator.Send(new GetAllUsersRequest());
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUserById(string id)
    {
        var response = await _mediator.Send(new GetUserByIdRequest { UserId = id });
        return response != null ? Ok(response) : NotFound();
    }

    [HttpPut("update")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> DeleteUser(string id)
    {
        var response = await _mediator.Send(new DeleteUserRequest { UserId = id });
        return Ok(response);
    }


}
