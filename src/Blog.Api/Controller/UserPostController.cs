using System.Security.Claims;
using Blog.Application.UserPost.Command.AddUserPost;
using Blog.Application.UserPost.Command.DeleteUserPost;
using Blog.Application.UserPost.Command.UpdateUserPost;
using Blog.Application.UserPost.Queries.GetAllUserPost;
using Blog.Application.UserPost.Queries.GetUserPostById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller;

[ApiController]
[Route("[action]")]
public class UserPostController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddUserPost([FromBody] AddUserPostCommand command)
    {
        var user = User;
        command.UserId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
        var result = await mediator.Send(command);
        return CreatedAtAction(null, new { id = result }, command);
    }

    [HttpGet] [Authorize] public async Task<IActionResult> GetAllUserPosts()
    {
        var result = await mediator.Send(new GetAllUserPostCommand());
        return Ok(result);

    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public async Task<IActionResult> GetUserPostById([FromRoute] string id)

{
        var result=await mediator.Send(new GetUserPostByIdCommand(id));
        if(result==null)
            return NotFound();
        return Ok(result);
        
    }
    [HttpPut("{postId}")]
    [Authorize]
    
    public async Task<IActionResult> UpdateUserPost([FromBody] UpdateUserPostCommand request,[FromRoute]string postId)
    {
        request.PostId = postId;
        var result=await mediator.Send(request);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
        
    }

    [HttpDelete]
    [Authorize]
    [Route("{postId}")]
    public async Task<IActionResult> DeleteUserPostById([FromRoute] string postId)
    {
       var result=await mediator.Send(new DeleteUserPostCommand(postId));
       if (result != null)
       {
           return NoContent();
       }
       return NotFound();
    }
    
}