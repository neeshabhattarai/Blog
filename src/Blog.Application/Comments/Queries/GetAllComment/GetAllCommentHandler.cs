using AutoMapper;
using Blog.Application.Comments.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.Comments.Queries.GetAllComment;

public class GetAllCommentHandler(ICommentText commentText,IMapper mapper):IRequestHandler<GetAllCommentCommand,List<ReadCommentDTO>>
{
    public async Task<List<ReadCommentDTO>> Handle(GetAllCommentCommand request, CancellationToken cancellationToken)
    {
        var commentList = commentText.GetAllComment();
        var listComment = mapper.Map<List<ReadCommentDTO>>(commentList);
        return listComment;
    }
}