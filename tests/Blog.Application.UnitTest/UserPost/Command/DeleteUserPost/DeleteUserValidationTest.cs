using Blog.Application.UserPost.Command.DeleteUserPost;
using Blog.Domain.Repository;
using Moq;
using NUnit.Framework;

namespace Test.UserPost.Command.DeleteUserPost;
[TestFixture]
public class DeleteUserValidationTest
{
  
  public DeleteUserPostHandler deleteUserPostHandler;
  public Mock<IUserPost> post;
  
  [SetUp]
  public void Setup()
  {
    post = new Mock<IUserPost>();
    deleteUserPostHandler = new DeleteUserPostHandler(post.Object);
  }
  [Test]
  public async Task DeleteUserPost_ShouldReturnTrue_WhenUserPostDeleted()
  {
    var deleteUserPostCommand = new DeleteUserPostCommand("22434");
    post.Setup(x => x.DeletePost(deleteUserPostCommand.PostId)).ReturnsAsync(true);
   var res=await deleteUserPostHandler.Handle(deleteUserPostCommand, CancellationToken.None);
   Assert.IsTrue(res);
   post.Verify(x => x.DeletePost(deleteUserPostCommand.PostId), Times.Once);
  }

  [Test]
  public async Task DeleteUserPost_ShouldReturnFalse_WhenUserPostNotDeleted()
  {
    var deleteUserPostCommand = new DeleteUserPostCommand("afdfe");
    post.Setup(x => x.DeletePost(deleteUserPostCommand.PostId)).ReturnsAsync(false);
    var res=await deleteUserPostHandler.Handle(deleteUserPostCommand, CancellationToken.None);
    Assert.IsFalse(res);
    post.Verify(x => x.DeletePost(deleteUserPostCommand.PostId), Times.Once);
  }
  
  [Test]
  public async Task DeleteUserPost_ShouldReturnNull_WhenUserPostNotFound()
  {
    var deleteUserPostCommand = new DeleteUserPostCommand("afdfe");
    post.Setup(x => x.DeletePost(deleteUserPostCommand.PostId)).ReturnsAsync((bool?)null);
    var res=await deleteUserPostHandler.Handle(deleteUserPostCommand, CancellationToken.None);
    Assert.IsNull(res);
    post.Verify(x => x.DeletePost(deleteUserPostCommand.PostId), Times.Once);
  }
}