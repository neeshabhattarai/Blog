using Blog.Application.Comments.Command.AddComment;
using Blog.Application.Comments.Command.DeleteComment;
using Blog.Application.Comments.Command.UpdateComment;
using Blog.Application.Comments.DTO;
using Blog.Application.Comments.Queries.GetAllComment;
using Blog.Application.Comments.Queries.GetById;
using Blog.Application.UserPost.DTO;
using Blog.Controller;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.Api.UnitTest;

public class CommentControllerTest
{
    public Mock<IMediator> _mediator { get; set; }
    public CommentsController _commentController { get; set; }
    [SetUp]
    public void Setup()
    {
        _mediator=new Mock<IMediator>();
    }

    [Test]
    public async Task CommentController_AddComment_ShouldReturnUnAuthorized()
    {
     _commentController=new CommentsController(_mediator.Object);
    var result=await _commentController.AddComment(new AddCommentCommand
     {
         Comment = "Test",
         PostId = "222"
     });
    
    Assert.IsInstanceOf<UnauthorizedResult>(result);
    var response=result as UnauthorizedResult;
    Assert.That(response.StatusCode,Is.EqualTo(401));
     
    }

    [Test]
    public async Task CommentController_GetCommentById_ShouldReturnComment()
    {
        _commentController=new CommentsController(_mediator.Object);
        _mediator.Setup(x => x.Send(It.IsAny<GetCommentByIdCommand>(),It.IsAny<CancellationToken>())).ReturnsAsync(new ReadCommentDTO
        {
            CommentId = "222",
            Comment = "Test",
            PostName = "Hello"
        });
        var result=await _commentController.GetCommentById("222");
        Assert.IsInstanceOf<OkObjectResult>(result);
        _mediator.Verify(x => x.Send(It.IsAny<GetCommentByIdCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var comment=result as OkObjectResult;
        Assert.That(comment.StatusCode,Is.EqualTo(200));
    }
    [Test]
    public async Task CommentController_GetCommentById_ShouldNotFound()
    {
        _commentController=new CommentsController(_mediator.Object);
        _mediator.Setup(x => x.Send(It.IsAny<GetCommentByIdCommand>(),It.IsAny<CancellationToken>())).ReturnsAsync((ReadCommentDTO?)null);
        var result=await _commentController.GetCommentById("222");
        Assert.IsInstanceOf<NotFoundResult>(result);
        var comment=result as NotFoundResult;
        Assert.That(comment.StatusCode,Is.EqualTo(404));
    }
    [Test]
    public async Task CommentController_GetAllComment_ShouldReturnComment()
    {
        _commentController=new CommentsController(_mediator.Object);
        _mediator.Setup(x => x.Send(It.IsAny<GetAllCommentCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new PageResult<ReadCommentDTO>
            (pageNumber:3,pageSize:10,list:new List<ReadCommentDTO>(),orderBy:"CommentId",sortDirection:"asc"));
        var result=await _commentController.GetAllComment(new GetAllCommentCommand
        {
            orderBy = "CommentId",
            pageIndex = 1,
            pageSize = 10,
            sortDirection = "asc"
        });
        Assert.IsInstanceOf<OkObjectResult>(result);
        var comment=result as OkObjectResult;
        Assert.That(comment.StatusCode,Is.EqualTo(200));
    }
    [Test]
    public async Task CommentController_DeleteCommentById_ReturnNotFound()
    {
        _commentController=new CommentsController(_mediator.Object);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteCommentCommand>(),It.IsAny<CancellationToken>())).ReturnsAsync((bool?)null);
        var result=await _commentController.DeleteComment("222");
        _mediator.Verify(x => x.Send(It.IsAny<DeleteCommentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.IsInstanceOf<NotFoundResult>(result);
        var comment=result as NotFoundResult;
        Assert.That(comment.StatusCode,Is.EqualTo(404));
    }
    [Test]
    public async Task CommentController_DeleteCommentById_ReturnNoContent()
    {
        _commentController=new CommentsController(_mediator.Object);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteCommentCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((bool?)true);
        var result=await _commentController.DeleteComment("222");
        _mediator.Verify(x => x.Send(It.IsAny<DeleteCommentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.IsInstanceOf<NoContentResult>(result);
        var comment=result as NoContentResult;
        Assert.That(comment.StatusCode,Is.EqualTo(204));
    }
    [Test]
    public async Task CommentController_UpdateCommentById_ReturnUpdatedComment()
    {
        var updateCommentCommand = new UpdateCommentCommand
        {
            CommentId = "222",
            Comment = "Test",
        };
        _commentController=new CommentsController(_mediator.Object);
        _mediator.Setup(x => x.Send(It.IsAny<UpdateCommentCommand>(),It.IsAny<CancellationToken>())).ReturnsAsync(new ReadCommentDTO
        {
            Comment = updateCommentCommand.Comment,
            CommentId = updateCommentCommand.CommentId,
            PostName =  "Hello",
            UserName =  "Test"
        });
        var result=await _commentController.UpdateComment(updateCommentCommand,updateCommentCommand.CommentId);
        _mediator.Verify(x => x.Send(It.IsAny<UpdateCommentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.IsInstanceOf<OkObjectResult>(result);
        var comment=result as OkObjectResult;
        Assert.That(comment.StatusCode,Is.EqualTo(200));
    }
    [Test]
    public async Task CommentController_UpdateCommentById_ReturnNotFound()
    {
        _commentController=new CommentsController(_mediator.Object);
        _mediator.Setup(x => x.Send(It.IsAny<UpdateCommentCommand>()
            ,It.IsAny<CancellationToken>())).ReturnsAsync((ReadCommentDTO?)null);
        var result=await _commentController.UpdateComment(new UpdateCommentCommand
        {
            Comment =  "Test",
        },"222");
        _mediator.Verify(x => x.Send(It.IsAny<UpdateCommentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.IsInstanceOf<NotFoundResult>(result);
        var comment=result as NotFoundResult;
        Assert.That(comment.StatusCode,Is.EqualTo(404));
    }
}