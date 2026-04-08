using Blog.Application.Comments.DTO;
using MediatR;

namespace Blog.Application.Comments.Queries.GetAllComment;

public class GetAllCommentCommand:IRequest<List<ReadCommentDTO>>
{
    
}