using System.Security.Claims;
using Blog.Application.UserPost.Command.AddUserPost;
using Blog.Application.UserPost.Queries.GetAllUserPost;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller;
[ApiController]
[Route("[action]")]
public class UserPostController(IMediator mediator):ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddUserPost([FromBody]AddUserPostCommand command)
    {
        var user = User;
        command.UserId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
        var result = await mediator.Send(command);
        return CreatedAtAction(null, new { id = result },command);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPosts()
    {
        var result=await mediator.Send(new GetAllUserPostCommand());
        return Ok(result);
        
    }
    
}