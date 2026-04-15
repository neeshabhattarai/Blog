using AutoMapper;
using Blog.Application.Comments.Command.AddComment;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Moq;
using Xunit;

namespace Test.Comments.Command.AddComment;

public class AddCommentHandlerTest
{
    public Mock<IMapper> mapper { get; set; }
    public Mock<ICommentText> commentText { get; set; }
    public AddCommentHandlerTest()
    {
        mapper=new Mock<IMapper>();
        commentText=new Mock<ICommentText>();
    }
    [Fact]
    public async Task AddCommentHandler_ShouldReturnSuccess()
    {
        var newCommentText = new CommentText
        {
            Comment = "Test",
            PostId = "33",
            UserId = "22",
            CommentId = "2232"
        };
       ;
        mapper.Setup(x => x.Map<CommentText>(It.IsAny<AddCommentCommand>())).Returns(newCommentText);
        commentText.Setup(s => s.AddComment(newCommentText)).ReturnsAsync(newCommentText.CommentId);
        var handler = new AddCommentHanlder(commentText.Object, mapper.Object);
        Assert.Equal(await handler.Handle(It.IsAny<AddCommentCommand>(),CancellationToken.None),"2232");
    }

    [Fact]
    public async Task AddCommentHandler_ShouldReturnNull()
    {
       mapper.Setup(x => x.Map<CommentText>(It.IsAny<AddCommentCommand>())).Returns(new CommentText());
       var handler = new AddCommentHanlder(commentText.Object, mapper.Object);
       var result = await handler.Handle(It.IsAny<AddCommentCommand>(), CancellationToken.None);
       Assert.Null(result);
    }
    [Fact]
    public async Task AddCommentHandler_ShouldReturnFailure()
    {
        var addCommentCommand = new AddCommentCommand();
        mapper.Setup(x => x.Map<CommentText>(addCommentCommand)).Returns(new CommentText());
        var handler = new AddCommentHanlder(commentText.Object, mapper.Object);
        commentText.Setup(x=>x.AddComment(It.IsAny<CommentText>())).ThrowsAsync(new Exception("User cannot be null"));
       var rs=await Assert.ThrowsAsync<Exception>(()=>handler.Handle(It.IsAny<AddCommentCommand>(), CancellationToken.None));
       Assert.Equal(rs.Message,"User cannot be null");
    }
}