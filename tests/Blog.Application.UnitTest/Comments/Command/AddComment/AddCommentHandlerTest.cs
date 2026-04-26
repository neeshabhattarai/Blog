using AutoMapper;
using Blog.Application.Comments.Command.AddComment;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Moq;
using NUnit.Asserts;
using NUnit.Framework;


namespace Test.Comments.Command.AddComment;
[TestFixture]
public class AddCommentHandlerTest
{
    public Mock<IMapper> mapper { get; set; }
    public Mock<ICommentCommand> commentText { get; set; }
    [SetUp]
    public void AddCommentHandlerTes()
    {
        mapper=new Mock<IMapper>();
        commentText=new Mock<ICommentCommand>();
    }
    [Test]
    public async Task AddCommentHandler_ShouldReturnSuccess()
    {
        var newCommentText = new CommentText
        {
            CommentId = "2232"
        };
        var addCommand = new AddCommentCommand
        {Comment = "Test",
            PostId = "33",
            UserId = "22",
            
        };
       
        mapper.Setup(x => x.Map<CommentText>(addCommand)).Returns(newCommentText);
        commentText.Setup(s => s.AddComment(newCommentText)).ReturnsAsync(newCommentText.CommentId);
        var handler = new AddCommentHanlder(commentText.Object, mapper.Object);
        Assert.AreEqual(await handler.Handle(addCommand,CancellationToken.None),"2232");
        commentText.Verify(x=>x.AddComment(newCommentText),Times.Once);
    }

    [Test]
    public async Task AddCommentHandler_ShouldReturnNull()
    {
       mapper.Setup(x => x.Map<CommentText>(It.IsAny<AddCommentCommand>())).Returns(new CommentText());
       var handler = new AddCommentHanlder(commentText.Object, mapper.Object);
       var result = await handler.Handle(It.IsAny<AddCommentCommand>(), CancellationToken.None);
       Assert.That(result,Is.Null);
    }
    [Test]
    public async Task AddCommentHandler_ShouldReturnFailure()
    {
        var addCommentCommand = new AddCommentCommand();
        mapper.Setup(x => x.Map<CommentText>(addCommentCommand)).Returns(new CommentText());
        var handler = new AddCommentHanlder(commentText.Object, mapper.Object);
        commentText.Setup(x=>x.AddComment(It.IsAny<CommentText>())).ThrowsAsync(new Exception("User cannot be null"));
       var rs= Assert.ThrowsAsync<Exception>(()=>handler.Handle(It.IsAny<AddCommentCommand>(), CancellationToken.None));
       Assert.AreEqual(rs.Message,"User cannot be null");
    }
}