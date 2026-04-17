using Blog.Domain.Entities;

namespace Blog.Domain.Repository;

public interface IUserPost
{
    
    Task<UserPost> GetUserPostById(string userId);
    Task<List<UserPost>> GetAllPost(string? search,int pageIndex, int pageSize,string? orderBy,string sortDirection);
    Task<string> AddPost(UserPost userPost);
    Task<UserPost> UpdatePost(UserPost userPost);
    Task<bool?> DeletePost(string postId);
}