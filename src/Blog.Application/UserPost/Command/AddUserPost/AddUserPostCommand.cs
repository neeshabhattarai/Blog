using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.UserPost.Command.AddUserPost;

public class AddUserPostCommand:IRequest<string>
{
    [JsonIgnore]
    public string? UserId { get; set; }
    public string PostTitle { get; set; }
    
}