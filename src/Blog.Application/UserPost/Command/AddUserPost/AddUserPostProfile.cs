using AutoMapper;
using Blog.Application.UserPost.DTO;

namespace Blog.Application.UserPost.Command.AddUserPost;

public class AddUserPostProfile:Profile
{
    public AddUserPostProfile()
    {
        CreateMap<AddUserPostCommand,Domain.Entities.UserPost>().ForMember(x=>x.PostId,opt=>opt.Ignore()).ReverseMap();
        CreateMap<Domain.Entities.UserPost,ReadUserPost>().ForMember(x=>x.UserName,opt=>opt.MapFrom(o=>o.User.UserName)).ReverseMap();
    }
}