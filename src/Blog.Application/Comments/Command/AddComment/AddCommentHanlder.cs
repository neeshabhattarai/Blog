using AutoMapper;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.Comments.Command.AddComment;

public class AddCommentHanlder(ICommentText commentText,IMapper mapper):IRequestHandler<AddCommentCommand,string>
{
    public async Task<string> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var mapperResult = mapper.Map<CommentText>(request);
        return await commentText.AddComment(mapperResult);
    }
}