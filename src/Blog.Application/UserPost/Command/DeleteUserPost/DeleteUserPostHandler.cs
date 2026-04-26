using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.UserPost.Command.DeleteUserPost;

public class DeleteUserPostHandler(IUserPostCommand post):IRequestHandler<DeleteUserPostCommand,bool?>
{
    public async Task<bool?> Handle(DeleteUserPostCommand request, CancellationToken cancellationToken)
    {
       var result= await post.DeletePost(request.PostId);
       return result;
    }
}