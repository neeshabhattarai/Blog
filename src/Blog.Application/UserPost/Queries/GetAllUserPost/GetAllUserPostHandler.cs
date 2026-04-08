using AutoMapper;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.UserPost.Queries.GetAllUserPost;

public class GetAllUserPostHandler(IUserPost userPost,IMapper mapper):IRequestHandler<GetAllUserPostCommand,List<ReadUserPost>>
{
    public async Task<List<ReadUserPost>> Handle(GetAllUserPostCommand request, CancellationToken cancellationToken)
    {
       var UserPostList= userPost.GetAllPost();
       var list=mapper.Map<List<ReadUserPost>>(UserPostList);
       return list;
    }
}