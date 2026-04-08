using Blog.Domain.Entities;

namespace Blog.Domain.Repository;

public interface ICommentText
{
    Task<CommentText> GetCommentById(string commentId);
    List<CommentText> GetAllComment();
    Task<int> AddComment(CommentText comment);
    Task<CommentText> UpdateComment(CommentText commentText,string commentId);
}