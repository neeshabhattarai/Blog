using Blog.Domain.Entities;

namespace Blog.Domain.Repository;

public interface IUserPost
{
    
    Task<UserPost> GetUserPostById(string userId);
    List<UserPost> GetAllPost();
    Task<string> AddPost(UserPost userPost);
    Task<UserPost> UpdatePost(UserPost userPost);
    Task<bool?> DeletePost(string postId);
}