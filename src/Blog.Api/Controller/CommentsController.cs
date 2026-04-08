using System.Security.Claims;
using Blog.Application.Comments.Command.AddComment;
using Blog.Application.Comments.DTO;
using Blog.Application.Comments.Queries.GetAllComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller;
[ApiController]
[Route("api/[action]")]
public class CommentsController(IMediator mediator):ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddComment([FromBody] AddCommentCommand request)
    {
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        request.UserId = userID;
       var result=await mediator.Send(request);
       return CreatedAtAction(null,new {id=result},request);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllComment()
    {
        var result = await mediator.Send(new GetAllCommentCommand());
        return Ok(result);
    }
    
}