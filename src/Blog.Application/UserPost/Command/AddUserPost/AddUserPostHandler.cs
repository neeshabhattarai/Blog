using AutoMapper;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.UserPost.Command.AddUserPost;

public class AddUserPostHandler(IUserPost userPost,IMapper mapper):IRequestHandler<AddUserPostCommand,string>
{
    public async Task<string> Handle(AddUserPostCommand request, CancellationToken cancellationToken)
    {
        var userPosts=mapper.Map<Domain.Entities.UserPost>(request);

      var result =await userPost.AddPost(userPosts);
      return result;
    }
}