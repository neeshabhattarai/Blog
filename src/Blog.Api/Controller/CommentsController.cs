using System.Security.Claims;
using Blog.Application.Comments.Command.AddComment;
using Blog.Application.Comments.Command.DeleteComment;
using Blog.Application.Comments.Command.UpdateComment;
using Blog.Application.Comments.DTO;
using Blog.Application.Comments.Queries.GetAllComment;
using Blog.Application.Comments.Queries.GetById;
using Blog.Infastructure.Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller;
[ApiController]
[Authorize(Policy = Policy.IsAdminOrUser)]
// [Authorize(Policy = Policy.IsUser)]
[Route("api/[action]")]
public class CommentsController(IMediator mediator):ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddComment([FromBody] AddCommentCommand request)
    {
        var user = User;
        if (user==null)
        {
            return Unauthorized();
        }
        
        var userID = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        request.UserId = userID;
       var result=await mediator.Send(request);
       return CreatedAtAction(null,new {id=result},request);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllComment([FromQuery] GetAllCommentCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCommentById(string id)
    {
        var result = await mediator.Send(new GetCommentByIdCommand(id));
        if(result==null)
            return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentCommand request,string id)
    {
        request.CommentId = id;
        var result = await mediator.Send(request);
        if(result==null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteComment(string id)
    {
     var reult=  await mediator.Send(new DeleteCommentCommand(id));
     if(reult==null)
         return NotFound();
     return NoContent();

    }
    
}