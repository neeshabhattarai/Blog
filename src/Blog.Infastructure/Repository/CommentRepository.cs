using System.Linq.Expressions;
using Blog.Domain.Constant;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Blog.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infastructure.Repository;

public class CommentRepository(BlogDbContext dbContext):ICommentText
{
    public async Task<CommentText> GetCommentById(string commentId)
    {
        var comment=await dbContext.CommentTexts.FirstOrDefaultAsync(x=>x.CommentId==commentId);
        if(comment==null)
            return null;
        return comment;
    }

    public  List<CommentText> GetAllComment(string? search,int pageIndex, int pageSize,string? orderBy,string sortDirection)
    {
        var queryable = dbContext.CommentTexts.Include("User").Include("Post").Where(x=>x.Comment==search || search==null);
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            var OrderedDesc = new Dictionary<string, Expression<Func<CommentText, object>>>
            {
                { nameof(CommentText.CommentId), x => x.CommentId },
                { nameof(CommentText.Comment), x => x.Comment }
            };
            queryable=SortingDirection.desc==sortDirection?queryable.OrderByDescending(OrderedDesc[orderBy]):queryable.OrderBy(OrderedDesc[orderBy]);
        }
        
        queryable=queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return queryable.ToList();
    }

    public async Task<string> AddComment(CommentText comment)
    { 
        await dbContext.CommentTexts.AddAsync(comment);
      await dbContext.SaveChangesAsync();
      return comment.CommentId.ToString();
    }
    public async Task<CommentText> UpdateComment(CommentText comment)
    {
        var comments = await GetCommentById(comment.CommentId);
        if (comments == null)
        {
            return null;
        }

        comments.Comment = comment.Comment;
        await  dbContext.SaveChangesAsync();
        return comments;
    }

    public async Task<bool?> DeleteComment(string requestId)
    {
        var comments = await GetCommentById(requestId);
        if (comments == null)
            return null;
        dbContext.CommentTexts.Remove(comments);
        await dbContext.SaveChangesAsync();
        return true;
    }
}