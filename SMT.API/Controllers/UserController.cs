using Microsoft.AspNetCore.Mvc;
using SMT.Application.User.RegisterUser;
using MediatR;
using SMT.Application.UserCommand.LoginUser;

namespace SMT.API.Controllers;

[Controller]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IMediator _mediator; 

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var result =  await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            var result =  await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}