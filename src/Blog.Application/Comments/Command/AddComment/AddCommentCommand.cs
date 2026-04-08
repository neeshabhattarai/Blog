using MediatR;

namespace Blog.Application.Comments.Command.AddComment;

public class AddCommentCommand:IRequest<string>
{
    public string Comment { get; set; }
  
}