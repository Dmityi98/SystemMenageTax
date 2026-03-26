using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.Years.GetYaerById;

namespace SMT.API.Controllers;

[Controller]
public class YearsController : Controller
{
    private readonly IMediator _mediator; 

    public YearsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("table/{id}")]
    public async Task<IActionResult> GetYearById(Guid id)
    {
        var command = new GetYearByIdCommand 
        { 
            Id = id 
        };

        var result = await _mediator.Send(command);
        
        if (result == null)
        {
            return NotFound();
        }
        return Ok();
    }
}