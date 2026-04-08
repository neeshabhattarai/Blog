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
        return comment;
    }

    public  List<CommentText> GetAllComment()
    {
        var queryable= dbContext.CommentTexts.Include("User").Include("Post").ToList(); 
        return queryable;
    }

    public async Task<string> AddComment(CommentText comment)
    { 
        await dbContext.CommentTexts.AddAsync(comment);
      await dbContext.SaveChangesAsync();
      return comment.CommentId.ToString();
    }
    public async Task<CommentText> UpdateComment(CommentText comment,string commentId)
    {
        var comments = await GetCommentById(commentId);
        if (comments == null)
        {
            return null;
        }

        comments.CommentId = comment.Comment;
        await  dbContext.SaveChangesAsync();
        return comments;
    }
}