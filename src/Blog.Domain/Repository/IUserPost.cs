using Blog.Domain.Entities;

namespace Blog.Domain.Repository;

public interface IUserPost
{
    
    Task<UserPost> GetUserPostById(string userId);
    List<UserPost> GetAllPost();
    Task<int> AddPost(UserPost userPost);
    Task<UserPost> UpdatePost(UserPost userPost,string postId);
}