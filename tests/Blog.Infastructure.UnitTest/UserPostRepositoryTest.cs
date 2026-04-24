using Blog.Application.UserPost.Command.UpdateUserPost;
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
    public User user{ get; set; }
    [SetUp]
    public void Setup()
    {
        user = new User
        {
            Id = "2222",
            UserName = "test"
        };

        _userPost1 = new UserPost
        {
            CommentTexts = new List<CommentText>
            {
                new CommentText { Comment = "Comment 1", CommentId = Guid.NewGuid().ToString(),UserId = user.Id},
                new CommentText { Comment = "Comment 2", CommentId = Guid.NewGuid().ToString(),UserId = user.Id },
            },
            PostId = "9999",
            User=user,
            UserId = user.Id,
            PostTitle = "Hello world"
        };
        _userPost2 = new UserPost
        {
            CommentTexts = new List<CommentText>
            {
                new CommentText { Comment = "Comment 3", CommentId = Guid.NewGuid().ToString() ,UserId = user.Id},
            },
            PostId = "1111",
            User =  user,
            UserId = user.Id,
            PostTitle = "Hello world2"
        };
    }

    [Test]
    public async Task GetAllUserPost_ShouldReturnAllUserPost()
    {
        var dbContextBuilder=new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(dbContextBuilder))
        { 
            userPostRepository = new UserPostRepository(context);
            context.Users.Add(user);
           await userPostRepository.AddPost(_userPost1);
           await userPostRepository.AddPost(_userPost2);
           var count=await context.UserPosts.CountAsync();
           
        }

        using (var context = new BlogDbContext(dbContextBuilder))
        {
             userPostRepository = new UserPostRepository(context);
            var result=await userPostRepository.GetAllPost(null, 1, 5, "PostTitle", "asc");
            Assert.Multiple(() =>
            {
                Assert.NotNull(result);
                Assert.AreEqual(_userPost1.PostId, result[0].PostId);
                Assert.That(_userPost2.PostId,Is.EqualTo( result[1].PostId));
                Assert.That(result.Select(x => x.PostTitle), Is.Ordered);
            });
        }
        }
    
    [Test]
    public async Task GetAllUserPost_ShouldReturnNull_WhenDataNotPresent()
    {
     var dbContextBuilder=new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(dbContextBuilder))
        {
            userPostRepository = new UserPostRepository(context);
            var result=await userPostRepository.GetAllPost(null, 1, 5, "PostTitle", "asc");
            Assert.IsEmpty(result);
        }
    }

    [Test]
    public async Task GetUserPostById_ShouldReturnPost()
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
            var result = await userPostRepository.GetUserPostById(_userPost1.PostId);
            Assert.NotNull(result);
            Assert.That(result.PostId, Is.EqualTo(_userPost1.PostId));
        } 
        
    }
    [Test]
    public async Task GetUserPostById_ShouldReturnNull()
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
        var dbContextBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var postId=_userPost1.PostId;
        using (var context = new BlogDbContext(dbContextBuilder))
        {
            userPostRepository = new UserPostRepository(context);
            context.Users.Add(user);
            await userPostRepository.AddPost(_userPost1);
            await userPostRepository.AddPost(_userPost2);
            var count=context.UserPosts.Count();
        }
        using (var context = new BlogDbContext(dbContextBuilder))
        {
            userPostRepository = new UserPostRepository(context);
            var result=await userPostRepository.DeletePost(postId);
            Assert.True(result);
            Assert.That(context.UserPosts.Count(),Is.EqualTo(1));
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
    [Test]
    public async Task AddUserPost_ShouldSuccess()
    {
        var db=new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(db))
        {
            userPostRepository = new UserPostRepository(context);
            var result=await userPostRepository.AddPost(_userPost1);
            Assert.AreEqual(1,context.UserPosts.Count());
            Assert.AreEqual(_userPost1.PostId,result);
        }
    }
    [Test]
    public async Task AddUserPost_ShouldThrowException_WhenInvalidInput()
    {
        var db=new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(db))
        {
            userPostRepository = new UserPostRepository(context);
            var post = new UserPost
            {
                PostId = _userPost1.PostId,
                PostTitle = _userPost1.PostTitle
            };
          var result=Assert.ThrowsAsync<DbUpdateException>(() => userPostRepository.AddPost(post));
          Assert.That(result,Is.TypeOf(typeof(DbUpdateException)));
        }
    }
    
    [Test]
    
    public async Task UpdateUserPost_ShouldSuccess()
    {
        var db=new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using (var context = new BlogDbContext(db))
        {
            userPostRepository = new UserPostRepository(context);
            await userPostRepository.AddPost(_userPost1);
        }

        using (var context = new BlogDbContext(db))
        {
            userPostRepository = new UserPostRepository(context);
            _userPost1.PostTitle = "Hello Test";
            var result=await userPostRepository.UpdatePost(_userPost1);
            Assert.AreEqual(_userPost1.PostTitle,result.PostTitle);
        }
        
    }
    [Test]
    public async Task UpdateUserPost_ShouldReturnNotFound()
    {
        var db=new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
      

        using (var context = new BlogDbContext(db))
        {
            userPostRepository = new UserPostRepository(context);
            _userPost1.PostTitle = "Hello Test";
            var result=await userPostRepository.UpdatePost(_userPost1);
            Assert.Null(result);
        }
        
    }
    // [Test]
    //
    // public async Task UpdateUserPost_WithInvalidInput_ShouldThrowException()
    // {
    //     var db=new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
    //     using (var context = new BlogDbContext(db))
    //     {
    //         userPostRepository = new UserPostRepository(context);
    //         await userPostRepository.AddPost(_userPost1);
    //     }
    //
    //     using (var context = new BlogDbContext(db))
    //     {
    //         userPostRepository = new UserPostRepository(context);
    //         var postT = new UserPost
    //         {
    //             PostId = _userPost1.PostId,
    //             UserId = null
    //         };
    //         var result=Assert.ThrowsAsync<DbUpdateException>(() => userPostRepository.UpdatePost(postT));
    //         Assert.That(result,Is.TypeOf(typeof(DbUpdateException)));
    //     }
    //     
    // }
    
    
}