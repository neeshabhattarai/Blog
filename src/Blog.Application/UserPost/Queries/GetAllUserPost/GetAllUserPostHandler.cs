using AutoMapper;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.UserPost.Queries.GetAllUserPost;

public class GetAllUserPostHandler(IUserPost userPost,IMapper mapper):IRequestHandler<GetAllUserPostCommand,PageResult<ReadUserPostDTO>>
{
    public async Task<PageResult<ReadUserPostDTO>> Handle(GetAllUserPostCommand request, CancellationToken cancellationToken)
    {
       var UserPostList=await userPost.GetAllPost(request.searchText, request.pageIndex, request.pageSize,request.orderBy,request.sortDirection);
       var list=mapper.Map<List<ReadUserPostDTO>>(UserPostList);
       return new PageResult<ReadUserPostDTO>(list,request.pageSize,request.pageIndex,request.orderBy,request.sortDirection);
    }
}