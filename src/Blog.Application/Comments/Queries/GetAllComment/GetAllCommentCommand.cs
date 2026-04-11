using Blog.Application.Comments.DTO;
using Blog.Application.UserPost.DTO;
using MediatR;

namespace Blog.Application.Comments.Queries.GetAllComment;

public class GetAllCommentCommand:IRequest<PageResult<ReadCommentDTO>>
{
    public string? searchText{get;set;}
    public int pageIndex{get;set;}

    public int pageSize{get;set;}
    public string? orderBy {get;set;}
    public string sortDirection {get;set;}
}