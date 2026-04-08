using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Blog.Infastructure.Data;

public class BlogDbContext:IdentityDbContext<User>
{
    public  BlogDbContext(DbContextOptions options):base(options)
    {
    }
    public DbSet<CommentText> CommentTexts { get; set; }
    public DbSet<UserPost> UserPosts { get; set; }
}