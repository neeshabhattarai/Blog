using AutoMapper;
using Blog.Application.Comments.Command.AddComment;
using Blog.Application.Comments.Command.UpdateComment;
using Blog.Domain.Entities;

namespace Blog.Application.Comments.DTO;

public class CommentAutoMapper:Profile
{
    public CommentAutoMapper()
    {
        CreateMap<CommentText, ReadCommentDTO>()
            .ForMember(x=>x.PostName,opt=>opt.MapFrom(z=>z.Post.PostTitle))
            .ForMember(x=>x.UserName,opt=>opt.MapFrom(z=>z.User.UserName)).ReverseMap();
        CreateMap<CommentText, AddCommentCommand>().ReverseMap();
        CreateMap<CommentText, UpdateCommentCommand>().ReverseMap();
    }
}