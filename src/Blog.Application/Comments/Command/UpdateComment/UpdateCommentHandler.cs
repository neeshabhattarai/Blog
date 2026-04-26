using AutoMapper;
using Blog.Application.Comments.DTO;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.Comments.Command.UpdateComment;

public class UpdateCommentHandler(ICommentCommand commentText,IMapper mapper):IRequestHandler<UpdateCommentCommand,ReadCommentDTO?>
{
    public async Task<ReadCommentDTO?> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var mapperTest=mapper.Map<CommentText>(request);
       var result= await commentText.UpdateComment(mapperTest);
       var resultMapper=mapper.Map<ReadCommentDTO>(result);
       return resultMapper;
    }
}