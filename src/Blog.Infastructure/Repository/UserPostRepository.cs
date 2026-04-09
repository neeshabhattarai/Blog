using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Blog.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infastructure.Repository;

public class UserPostRepository(BlogDbContext context):IUserPost
{
    public async Task<UserPost> GetUserPostById(string postId)
    {
        var userPost=await context.UserPosts.Include(u=>u.CommentTexts).Include(u=>u.User)
            .FirstOrDefaultAsync(u=>u.PostId == postId);
        if (userPost == null)
            return null;
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

    public async Task<UserPost> UpdatePost(UserPost userPost)
    {
        var userPosts = await GetUserPostById(userPost.PostId);
        if (userPosts == null)
        {
            return null;
        }

        userPosts.PostTitle = userPost.PostTitle;
        await  context.SaveChangesAsync();
        return userPosts;
    }
    public async Task<bool?> DeletePost(string postId)
    {
        var userPost = await GetUserPostById(postId);
        if (userPost == null)
            return null;
         context.UserPosts.Remove(userPost);
         await context.SaveChangesAsync();
         return true;
    }
}