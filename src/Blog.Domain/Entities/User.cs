using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities;

public class User:IdentityUser
{
    public ICollection<UserPost> UserPosts { get; set; }=new List<UserPost>();
}