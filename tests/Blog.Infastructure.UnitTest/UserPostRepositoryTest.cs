using Blog.Domain.Entities;
using Blog.Infastructure.Data;
using Blog.Infastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infastructure.UnitTest;
[TestFixture]
public class UserPostRepositoryTest
{
    private UserPost _userPost1 { get; set; }
    private UserPost _userPost2 { get; set; }
    public UserPostRepository userPostRepository { get; set; }
    [SetUp]
    public void Setup()
    {
        _userPost1 = new UserPost
        {
            // CommentTexts = new List<CommentText>
            // {
            //     new CommentText { Comment = "Comment 1", CommentId = Guid.NewGuid().ToString(),UserId = "2222"},
            //     new CommentText { Comment = "Comment 2", CommentId = Guid.NewGuid().ToString(),UserId = "2222" },
            // },
            PostId = Guid.NewGuid().ToString(),
            UserId = "2222",
            PostTitle = "Hello world"
        };
        _userPost2 = new UserPost
        {
            // CommentTexts = new List<CommentText>
            // {
            //     new CommentText { Comment = "Comment 3", CommentId = Guid.NewGuid().ToString() ,UserId = "3333"},
            // },
            PostId = Guid.NewGuid().ToString(),
            UserId = "3333",
            PostTitle = "Hello world2"
        };
    }

    [Test]
    public async Task GetUserPostById_ShouldReturnAllComments()
    {
        var dbContextBuilder=new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(dbContextBuilder))
        { 
            userPostRepository = new UserPostRepository(context);
           await userPostRepository.AddPost(_userPost1);
           await userPostRepository.AddPost(_userPost2);
        }

        using (var context = new BlogDbContext(dbContextBuilder))
        {
             userPostRepository = new UserPostRepository(context);
            var result = await userPostRepository.GetUserPostById("2222");
            Assert.NotNull(result);
        }
        }

    [Test]
    public async Task GetUserPostByUserId_ShouldReturnNull()
    {
        var dbContextBuilder=new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(dbContextBuilder))
        {
            userPostRepository = new UserPostRepository(context);
            var result = await userPostRepository.GetUserPostById("2222");
            Assert.Null(result);
        } 
        
    }

    [Test]
    public async Task DeleteUserPost_ShouldReturnNoContent()
    {
        var dbContextBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(dbContextBuilder))
        {
            userPostRepository = new UserPostRepository(context);
            await userPostRepository.AddPost(_userPost1);
            await userPostRepository.AddPost(_userPost2);
        }

        using (var context = new BlogDbContext(dbContextBuilder))
        {
            userPostRepository = new UserPostRepository(context);
            var result=await userPostRepository.DeletePost(_userPost1.PostId);
            Assert.True(result);
        }
    }
    [Test]
    public async Task DeleteUserPost_ShouldReturnNotFound()
    {
        var dbContextBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString()).Options;
        
        using (var context = new BlogDbContext(dbContextBuilder))
        {
            userPostRepository = new UserPostRepository(context);
            var result=await userPostRepository.DeletePost(_userPost1.PostId);
            Assert.Null(result);
        }
    }
    
}