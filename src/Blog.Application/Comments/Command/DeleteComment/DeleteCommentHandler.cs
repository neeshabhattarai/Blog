using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.Comments.Command.DeleteComment;

public class DeleteCommentHandler(ICommentCommand commentText):IRequestHandler<DeleteCommentCommand,bool?>
{
    public async Task<bool?> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
      var deleteResult= await commentText.DeleteComment(request.Id);
      return deleteResult;
    }
}