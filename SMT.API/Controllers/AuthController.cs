using MediatR;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.Auth.LoginUser;
using SMT.Application.UserCommand.RefreshToken;
using SMT.Application.UserCommand.RegisterUser;

namespace SMT.API.Controllers;
[Route("api/[controller]")]
public class AuthController(IMediator _mediator) : Controller
{
    /// <summary>
    /// Регистрация пользователя с RegisterUserCommand
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    /// <summary>
    /// Аторизация пользователя и получение jwt token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    /// <summary>
    /// Обновление jwt token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}