using System.Linq.Expressions;
using Blog.Domain.Constant;
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

    public List<UserPost> GetAllPost(string? search,int pageIndex, int pageSize,string? orderBy,string sortDirection)
    {
        var allPost = context.UserPosts.Include("User").Include(c=>c.CommentTexts)
            .Where(u=>u.PostTitle.Contains(search) || search==null);

        if (orderBy != null)
        {
            var OrderDictionary=new Dictionary<string,Expression<Func<UserPost, object>>>
            {
                {nameof(UserPost.PostId),u=>u.PostId},
                {nameof(UserPost.PostTitle),u=>u.PostTitle}
            };
            allPost = sortDirection == SortingDirection.desc
                ? allPost.OrderByDescending(OrderDictionary[orderBy])
                : allPost.OrderBy(OrderDictionary[orderBy]);
        }

       allPost=allPost.Skip(pageIndex-1).Take(pageSize);
        return allPost.ToList();
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