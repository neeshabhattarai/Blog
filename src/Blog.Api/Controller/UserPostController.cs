using System.Security.Claims;
using Blog.Application.UserPost.Command.AddUserPost;
using Blog.Application.UserPost.Command.DeleteUserPost;
using Blog.Application.UserPost.Command.UpdateUserPost;
using Blog.Application.UserPost.Queries.GetAllUserPost;
using Blog.Application.UserPost.Queries.GetUserPostById;
using Blog.Infastructure.Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller;

[ApiController]
[Authorize(Policy = Policy.IsAdminOrUser)]
// [Authorize(Policy = Policy.IsUser)]
// [Authorize(Policy = Policy.IsAuthor)]
[Route("api/Post/[action]")]
public class UserPostController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    
    public async Task<IActionResult> AddUserPost([FromBody] AddUserPostCommand command)
    {
        var user = User;
        command.UserId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
        var result = await mediator.Send(command);
        return CreatedAtAction(null, new { id = result }, command);
    }
    [HttpGet]
    
   public async Task<IActionResult> GetAllUserPosts([FromQuery] GetAllUserPostCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);

    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserPostById([FromRoute] string id)

{
        var result=await mediator.Send(new GetUserPostByIdCommand(id));
            return NotFound();
        return Ok(result);
        
    }
    [HttpPut("{postId}")]
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