using System.Linq.Expressions;
using Blog.Application.UserPost.Command.AddUserPost;
using Blog.Domain.Constant;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Blog.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

namespace Blog.Infastructure.Repository;

public class UserPostRepository(BlogDbContext context):IUserPostCommand,IUserPostQuery
{
    public async Task<UserPost> GetUserPostById(string postId)
    {
        var userPost=await context.UserPosts.Include(u=>u.CommentTexts).Include(u=>u.User)
            .FirstOrDefaultAsync(u=>u.PostId == postId);
        if (userPost == null)
            return null;
        return userPost;
    }

    public Task<List<UserPost>> GetAllPost(string? search,int pageIndex, int pageSize,string? orderBy,string sortDirection)
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
        var pageLength = pageIndex<=0 ? 0 : pageIndex;
        var pageTaker = pageSize <= 0 ? allPost.Count() : pageSize;
       allPost=allPost.Skip((pageLength-1)*pageTaker).Take(pageTaker);
       var result = allPost.ToList();
        return allPost.ToListAsync();
    }

    public async Task<string> AddPost(UserPost userPost)
    {
        await context.UserPosts.AddAsync(userPost);
        await context.SaveChangesAsync();
        return userPost.PostId;
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