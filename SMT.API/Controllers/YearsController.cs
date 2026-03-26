using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.Years.CreateTable;
using SMT.Application.Years.GetYaerById;
using SMT.Application.Years.GetYearById;
using SMT.Application.Years.UpdateTable;

namespace SMT.API.Controllers;

[Authorize]
[Controller]
[Route("api/[controller]")]
public class YearsController : Controller
{
    private readonly IMediator _mediator; 

    public YearsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetYearById(Guid id)
    {
        try
        {
            var command = new GetYearByIdCommand()
            {
                Id = id
            };
            var result = await _mediator.Send(command);
            
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the exception (in a real application, use a logger)
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] CreateTableCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTable([FromBody] UpdateTableCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}