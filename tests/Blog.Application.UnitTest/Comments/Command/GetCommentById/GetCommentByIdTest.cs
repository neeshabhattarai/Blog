using AutoMapper;
using Blog.Application.Comments.Command.DeleteComment;
using Blog.Application.Comments.DTO;
using Blog.Application.Comments.Queries.GetById;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Moq;
using NUnit.Framework;

namespace Test.Comments.Command.GetCommentById;

public class GetCommentByIdTest
{
    public Mock<ICommentText> commentTextMock { get; set; }
    public GetCommentByIdHandler GetCommentByIdHandler { get; set; }
    public Mock<IMapper> mapperMock { get; set; }
    [SetUp]
    public void Setup()
    {
        commentTextMock = new Mock<ICommentText>();
        mapperMock = new Mock<IMapper>();
        GetCommentByIdHandler=new(commentTextMock.Object,mapperMock.Object);
    }

    [Test]
    public async Task GetCommentById_ShouldReturnComment_WhenCommentExists()
    {
        var commentId = Guid.NewGuid().ToString();
        var comment = new CommentText
        {
            CommentId = commentId,
            Comment = "Comment Get",
            
        };
        var readComment = new ReadCommentDTO
        {
            CommentId = commentId,
            Comment = comment.Comment,
            PostName = "Hello World",
            UserName = "Test"
        };
        mapperMock.Setup(x => x.Map<ReadCommentDTO>(comment)).Returns(readComment);
        commentTextMock.Setup(x=>x.GetCommentById(commentId)).ReturnsAsync(comment);
        var result=await GetCommentByIdHandler.Handle(new (commentId), CancellationToken.None);
         Assert.IsNotNull(result);
         Assert.AreEqual(commentId, result.CommentId);
         Assert.That(result.Comment,Is.EquivalentTo(readComment.Comment));
         mapperMock.Verify(x => x.Map<ReadCommentDTO>(comment), Times.Once);
         commentTextMock.Verify(x => x.GetCommentById(commentId), Times.Once);
    }
    
    [Test]
    public async Task GetCommentById_ShouldReturnNull_WhenCommentDoesNotExists()
    {
        var commentId = Guid.NewGuid().ToString();
        commentTextMock.Setup(x=>x.GetCommentById(commentId)).ReturnsAsync((CommentText)null);
        var result=await GetCommentByIdHandler.Handle(new (commentId), CancellationToken.None);
        Assert.IsNull(result);
        commentTextMock.Verify(x => x.GetCommentById(commentId), Times.Once);
    }
    [Test]
    public async Task GetCommentById_ShouldReturnException_WhenCommentDoesNotExists()
    {
        var commentId = Guid.NewGuid().ToString();
        commentTextMock.Setup(x=>x.GetCommentById(commentId)).ThrowsAsync(new Exception("Comment Not Found"));
       var result= Assert.ThrowsAsync<Exception>(async () => await GetCommentByIdHandler.Handle(new (commentId), CancellationToken.None));
        Assert.AreEqual("Comment Not Found", result.Message);
        commentTextMock.Verify(x => x.GetCommentById(commentId), Times.Once);
    }
}