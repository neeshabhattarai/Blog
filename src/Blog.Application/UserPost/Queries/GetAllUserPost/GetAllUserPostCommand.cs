using Blog.Application.Comments.DTO;
using Blog.Application.UserPost.DTO;
using MediatR;

namespace Blog.Application.UserPost.Queries.GetAllUserPost;

public class GetAllUserPostCommand:IRequest<PageResult<ReadUserPostDTO>>
{ 
    public string? searchText{get;set;}
public int pageIndex{get;set;}

public int pageSize{get;set;}
public string? orderBy {get;set;}
public string sortDirection {get;set;}

}