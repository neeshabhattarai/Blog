using MediatR;

namespace Blog.Application.Comments.Command.AddComment;

public class AddCommentHanlder:IRequestHandler<AddCommentCommand,string>
{
    public async Task<string> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        return "Successfully Reached here";
    }
}