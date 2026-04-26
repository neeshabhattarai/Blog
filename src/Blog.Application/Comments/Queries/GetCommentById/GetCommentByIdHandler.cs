using AutoMapper;
using Blog.Application.Comments.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.Comments.Queries.GetById;

public class GetCommentByIdHandler(ICommentQuery commentText,IMapper mapper):IRequestHandler<GetCommentByIdCommand,ReadCommentDTO?>
{
    public async Task<ReadCommentDTO?> Handle(GetCommentByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await commentText.GetCommentById(request.Id);
        return mapper.Map<ReadCommentDTO>(result);
    }
}