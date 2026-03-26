using Microsoft.AspNetCore.Mvc;
using SMT.Application.User.RegusterUser;
using MediatR;
namespace SMT.API.Controllers;

[Controller]
public class UserController : Controller
{
    // GET
    private readonly IMediator _mediator; 

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result =  await _mediator.Send(command);
        return Ok(result);
    }
}