using Blog.Application.UserPost.DTO;
using MediatR;

namespace Blog.Application.UserPost.Queries.GetAllUserPost;

public class GetAllUserPostCommand:IRequest<List<ReadUserPost>>
{
    
}