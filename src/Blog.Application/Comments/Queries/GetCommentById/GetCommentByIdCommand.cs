using Blog.Application.Comments.DTO;
using MediatR;

namespace Blog.Application.Comments.Queries.GetById;

public class GetCommentByIdCommand(string id):IRequest<ReadCommentDTO?>
{
    public string? Id = id;
}