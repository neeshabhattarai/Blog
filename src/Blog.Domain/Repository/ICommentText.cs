using Blog.Domain.Entities;

namespace Blog.Domain.Repository;

public interface ICommentCommand
{
       Task<string> AddComment(CommentText comment);
    Task<CommentText> UpdateComment(CommentText commentText);
    Task<bool?> DeleteComment(string commentId);
}

public interface ICommentQuery
{
    Task<CommentText> GetCommentById(string commentId);
    Task<List<CommentText>> GetAllComment(string? search,int pageIndex, int pageSize,string? orderBy,string sortDirection);
}
