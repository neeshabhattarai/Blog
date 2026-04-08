using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Comments.Command.AddComment;

public class AddCommentCommand:IRequest<string>
{
    public string Comment { get; set; }
    public string PostId { get; set; }
    [JsonIgnore]
    public string? UserId { get; set; }
  
}