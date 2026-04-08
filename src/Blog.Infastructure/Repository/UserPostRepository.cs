using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Blog.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infastructure.Repository;

public class UserPostRepository(BlogDbContext context):IUserPost
{
    public async Task<UserPost> GetUserPostById(string userId)
    {
        var userPost=await context.UserPosts.Include("Comments").FirstOrDefaultAsync(u => u.UserId == userId);
        return userPost;
    }

    public List<UserPost> GetAllPost()
    {
        var allPost = context.UserPosts.Include("User").Include(u=>u.CommentTexts).ToList();
        return allPost;
    }

    public async Task<string> AddPost(UserPost userPost)
    {
        await context.UserPosts.AddAsync(userPost);
        await context.SaveChangesAsync();
        return userPost.UserId;
    }

    public async Task<UserPost> UpdatePost(UserPost userPost, string postId)
    {
        var userPosts = await GetUserPostById(postId);
        if (userPosts == null)
        {
            return null;
        }

        userPosts.CommentTexts = userPost.CommentTexts;
        await  context.SaveChangesAsync();
        return userPost;
        
    }
}