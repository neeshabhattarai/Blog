using Blog.Application.Comments.Command.AddComment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller;
[ApiController]
[Route("api/[action]")]
public class CommentsController(IMediator mediator):ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get()
    {
       var result=await mediator.Send(new AddCommentCommand());
       return Ok(result);
    }
    
}