using MediatR;

namespace Blog.Application.UserPost.Command.DeleteUserPost;

public class DeleteUserPostCommand(string postId):IRequest<bool?>
{
    public string PostId = postId;

}