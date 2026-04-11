using AutoMapper;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Repository;
using MediatR;

namespace Blog.Application.UserPost.Queries.GetUserPostById;

public class GetUserPostByIdHandler(IUserPost userPost,IMapper mapper):IRequestHandler<GetUserPostByIdCommand,ReadUserPostDTO>
{
    public async Task<ReadUserPostDTO> Handle(GetUserPostByIdCommand request, CancellationToken cancellationToken)
    {
       var result=await userPost.GetUserPostById(request.Id);
     var mappingResult=  mapper.Map<ReadUserPostDTO>(result);
     return mappingResult;
       
    }
}