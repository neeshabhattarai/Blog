using Blog.Application.UserPost.DTO;
using MediatR;

namespace Blog.Application.UserPost.Queries.GetUserPostById;

public class GetUserPostByIdCommand(string id):IRequest<ReadUserPost>
{
  public string Id  = id;
}