using MediatR;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.User.GetProfileUser;

namespace SMT.API.Controllers;


[Route("api/[controller]")]
public class UserProfileController(IMediator _mediator) : Controller
{
   [HttpGet]
   [Route("GetUserProfile")]
   public async Task<IActionResult> GetProfileUser([FromBody]GetProfileUserCommand  command)
   {
      var result = await _mediator.Send(command);
      return Ok(result);
   }
}