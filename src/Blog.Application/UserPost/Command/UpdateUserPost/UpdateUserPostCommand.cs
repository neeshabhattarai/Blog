using System.Text.Json.Serialization;
using Blog.Application.UserPost.DTO;
using MediatR;

namespace Blog.Application.UserPost.Command.UpdateUserPost;

public class UpdateUserPostCommand:IRequest<ReadUserPostDTO?>
{
    [JsonIgnore]
    public string? PostId {get;set;}
    public string PostTitle { get; set; }
    
}