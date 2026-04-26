using Blog.Domain.Entities;

namespace Blog.Domain.Repository;

public interface IUserPostQuery
{
    Task<UserPost> GetUserPostById(string userId);
    Task<List<UserPost>> GetAllPost(string? search,int pageIndex, int pageSize,string? orderBy,string sortDirection);
}

public interface IUserPostCommand
{
    Task<string> AddPost(UserPost userPost);
    Task<UserPost> UpdatePost(UserPost userPost);
    Task<bool?> DeletePost(string postId);
}

