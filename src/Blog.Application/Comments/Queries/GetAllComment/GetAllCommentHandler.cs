using AutoMapper;
using Blog.Application.Comments.DTO;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.Comments.Queries.GetAllComment;

public class GetAllCommentHandler(ICommentText commentText,IMapper mapper):IRequestHandler<GetAllCommentCommand,PageResult<ReadCommentDTO>>
{
    public async Task<PageResult<ReadCommentDTO>> Handle(GetAllCommentCommand request, CancellationToken cancellationToken)
    {
        var commentList = commentText.GetAllComment(request.searchText, request.pageIndex, request.pageSize,request.orderBy,request.sortDirection);
        var listComment = mapper.Map<List<ReadCommentDTO>>(commentList);
        return new PageResult<ReadCommentDTO>(listComment,request.pageSize,request.pageIndex,request.orderBy,request.sortDirection);
    }
}