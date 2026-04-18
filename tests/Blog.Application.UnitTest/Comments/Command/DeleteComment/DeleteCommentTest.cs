using Blog.Application.Comments.Command.DeleteComment;
using Blog.Domain.Repository;
using Moq;
using NUnit.Framework;

namespace Test.Comments.Command.DeleteComment;
[TestFixture]
public class DeleteCommentTest
{
    public Mock<ICommentText> commentTextMock;
    public DeleteCommentHandler deleteCommentHandler;
    [SetUp]
    public void Setup()
    {
        commentTextMock = new Mock<ICommentText>();
        deleteCommentHandler = new DeleteCommentHandler(commentTextMock.Object);
        
    }
  [Test]
  [TestCase("233",ExpectedResult = true)]
    public async Task<bool?> DeleteComment_ShouldReturnNoContent_WhenUserDeleteCommentIsSuccessful(string commentId)
    {
        var deleteCommand = new DeleteCommentCommand(commentId);
        commentTextMock.Setup(x=>x.DeleteComment(commentId)).ReturnsAsync(true);
        var result=await deleteCommentHandler.Handle(deleteCommand, CancellationToken.None);
        commentTextMock.Verify(x=>x.DeleteComment(commentId), Times.Once);
        return result;
    }
    [Test]
    [TestCase("299",ExpectedResult = null)]
    public async Task<bool?> DeleteComment_ShouldReturnNotFound_WhenUserDoesnotExists(string commentId)
    {
        var deleteCommand = new DeleteCommentCommand(commentId);
        commentTextMock.Setup(x=>x.DeleteComment(commentId)).ReturnsAsync((bool?)null);
        var result=await deleteCommentHandler.Handle(deleteCommand, CancellationToken.None);
        commentTextMock.Verify(x=>x.DeleteComment(commentId), Times.Once);
        return result;
    }

    [Test]
    [TestCase("293", ExpectedResult = "Cannot delete comment at this time")]
    public async Task<string> DeleteComment_ShouldReturnBadRequest_WhenUserDoesnotExists(string commentId)
    {
        var deleteCommand = new DeleteCommentCommand(commentId);
        commentTextMock.Setup(x => x.DeleteComment(commentId)).ThrowsAsync(new Exception("Cannot delete comment at this time"));
        var result = Assert.ThrowsAsync<Exception>(async ()=>await deleteCommentHandler.Handle(deleteCommand, CancellationToken.None));
        commentTextMock.Verify(x=>x.DeleteComment(commentId),Times.Once);
        return result.Message;
    }
    
    
}