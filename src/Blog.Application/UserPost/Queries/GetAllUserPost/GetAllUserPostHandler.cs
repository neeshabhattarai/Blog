using AutoMapper;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.UserPost.Queries.GetAllUserPost;

public class GetAllUserPostHandler(IUserPost userPost,IMapper mapper):IRequestHandler<GetAllUserPostCommand,PageResult>
{
    public async Task<PageResult> Handle(GetAllUserPostCommand request, CancellationToken cancellationToken)
    {
       var UserPostList= userPost.GetAllPost(request.searchText, request.pageIndex, request.pageSize,request.orderBy,request.sortDirection);
       var list=mapper.Map<List<ReadUserPost>>(UserPostList);
       return new PageResult(list,request.pageSize,request.pageIndex);
    }
}