using MediatR;

namespace Blog.Application.Comments.Command.DeleteComment;

public class DeleteCommentCommand(string id):IRequest<bool?>
{
 public string Id = id;
}