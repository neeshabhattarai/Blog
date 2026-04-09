using AutoMapper;
using Blog.Application.UserPost.Command.UpdateUserPost;
using Blog.Application.UserPost.DTO;

namespace Blog.Application.UserPost.Command.AddUserPost;

public class UserPostProfile:Profile
{
    public UserPostProfile()
    {
        CreateMap<UpdateUserPostCommand, Domain.Entities.UserPost>();
        CreateMap<AddUserPostCommand,Domain.Entities.UserPost>().ForMember(x=>x.PostId,opt=>opt.Ignore()).ReverseMap();
        CreateMap<Domain.Entities.UserPost,ReadUserPost>().ForMember(x=>x.UserName,opt=>opt.MapFrom(o=>o.User.UserName)).ReverseMap();
    }
}