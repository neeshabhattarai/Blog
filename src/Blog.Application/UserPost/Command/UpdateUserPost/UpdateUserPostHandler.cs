using AutoMapper;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.UserPost.Command.UpdateUserPost;

public class UpdateUserPostHandler(IUserPostCommand userPost,IMapper mapper):IRequestHandler<UpdateUserPostCommand,ReadUserPostDTO?>
{
    public async Task<ReadUserPostDTO?> Handle(UpdateUserPostCommand request, CancellationToken cancellationToken)
    {
        var mapperPost = mapper.Map<Domain.Entities.UserPost>(request);
        var post =await userPost.UpdatePost(mapperPost);
        var resultPost=mapper.Map<ReadUserPostDTO>(post);
        return resultPost;
    }
}