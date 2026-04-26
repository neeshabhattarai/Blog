using Blog.Application.UserPost.Command.DeleteUserPost;
using Blog.Domain.Repository;
using Moq;
using NUnit.Framework;

namespace Test.UserPost.Command.DeleteUserPost;
[TestFixture]
public class DeleteUserValidationTest
{
  
  public DeleteUserPostHandler deleteUserPostHandler;
  public Mock<IUserPostCommand> post;
  public DeleteUserPostCommand deleteUserPostCommand;
  
  [SetUp]
  public void Setup()
  {
    post = new Mock<IUserPostCommand>();
    deleteUserPostHandler = new DeleteUserPostHandler(post.Object);
    deleteUserPostCommand = new DeleteUserPostCommand("afdfe");
  }
  
  [Test]
  public async Task DeleteUserPost_ShouldReturnTrue_WhenUserPostDeleted()
  {
    post.Setup(x => x.DeletePost(It.IsAny<string>())).ReturnsAsync(true);
   var res=await deleteUserPostHandler.Handle(deleteUserPostCommand, CancellationToken.None);
   Assert.IsTrue(res);
   post.Verify(x => x.DeletePost(deleteUserPostCommand.PostId), Times.Once);
  }

  [Test]
  public async Task DeleteUserPost_ShouldReturnException_WhenDbConnectionFails()
  {
    post.Setup(x => x.DeletePost(It.IsAny<string>())).ThrowsAsync(new Exception("Db connection failure"));
    var res=Assert.ThrowsAsync<Exception>(async ()=>await deleteUserPostHandler.Handle(deleteUserPostCommand, CancellationToken.None));
    post.Verify(x => x.DeletePost(deleteUserPostCommand.PostId), Times.Once);
    Assert.AreEqual("Db connection failure", res.Message);
  }
  
  [Test]
  public async Task DeleteUserPost_ShouldReturnNull_WhenUserPostNotFound()
  {
    post.Setup(x => x.DeletePost(It.IsAny<string>())).ReturnsAsync((bool?)null);
    var res=await deleteUserPostHandler.Handle(new DeleteUserPostCommand(""),CancellationToken.None);
    Assert.IsNull(res);
    post.Verify(x => x.DeletePost(It.IsAny<string>()), Times.Once);
  }
}