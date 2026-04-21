using AutoMapper;
using Blog.Application.Comments.DTO;
using Blog.Application.UserPost.Command.UpdateUserPost;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Repository;
using Moq;
using NUnit.Framework;

namespace Test.UserPost.Command.UpdateUserPost;
[TestFixture]
public class UpdateUserPostHandlerTest
{
    public Mock<IUserPost> userPost;
    public UpdateUserPostHandler Handler;
    public Mock<IMapper> Mapper;
    public UpdateUserPostCommand updateRequest;
    public Blog.Domain.Entities.UserPost command;
    public ReadUserPostDTO readDto;
    [SetUp]
    public void Setup()
    {
        userPost=new Mock<IUserPost>();
        Mapper = new Mock<IMapper>();
        Handler = new UpdateUserPostHandler(userPost.Object,Mapper.Object);
         updateRequest = new UpdateUserPostCommand
        {
            PostId = "22",
            PostTitle = "Testing Title"
        }; 
        command = new Blog.Domain.Entities.UserPost
        {
            PostId = updateRequest.PostId,
            PostTitle = updateRequest.PostTitle
        };
        readDto = new ReadUserPostDTO
        {
            PostId = updateRequest.PostId,
            PostTitle = updateRequest.PostTitle,
            CommentTexts = new List<ReadCommentDTO>()
        };
    }

    [Test]
    public async Task UpdateUserPost_ShouldUpdateUserPost_WhenPostExists()
    {
        Mapper.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(updateRequest)).Returns(command);
        Mapper.Setup(x => x.Map<ReadUserPostDTO>(command)).Returns(readDto);
        userPost.Setup(x => x.UpdatePost(command)).ReturnsAsync(command);
        var result = await Handler.Handle(updateRequest, CancellationToken.None);
        Assert.That(result.PostTitle,Is.EquivalentTo(readDto.PostTitle));
        userPost.Verify(x => x.UpdatePost(command), Times.Once);
        Mapper.Verify(x => x.Map<ReadUserPostDTO>(command), Times.Once);
        Mapper.Verify(x => x.Map<Blog.Domain.Entities.UserPost>(updateRequest), Times.Once);
    }
    [Test]
    public async Task UpdateUserPost_ShouldReturnNull_WhenPostDoesNotExist()
    {
        Mapper.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(updateRequest)).Returns(command);
        Mapper.Setup(x => x.Map<ReadUserPostDTO>(command)).Returns((ReadUserPostDTO)null);
        userPost.Setup(x => x.UpdatePost(command)).ReturnsAsync((Blog.Domain.Entities.UserPost)null);
        var result = await Handler.Handle(updateRequest, CancellationToken.None);
        Assert.IsNull(result);
        userPost.Verify(x => x.UpdatePost(command), Times.Once);
        Mapper.Verify(x => x.Map<ReadUserPostDTO>(It.IsAny<Blog.Domain.Entities.UserPost>()), Times.Once);
        Mapper.Verify(x => x.Map<Blog.Domain.Entities.UserPost>(updateRequest), Times.Once);
    }

    [Test]
    public async Task UpdateUserPost_ShouldReturnException_WhenRepositoryFails()
    {
        Mapper.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(updateRequest)).Returns(command);
        userPost.Setup(x => x.UpdatePost(command)).ThrowsAsync(new Exception("Db Connection failed."));
        var result=Assert.ThrowsAsync<Exception>(async () => await Handler.Handle(updateRequest, CancellationToken.None));
        Assert.That(result.Message,Is.EqualTo("Db Connection failed."));
        userPost.Verify(x => x.UpdatePost(command), Times.Once);
        Mapper.Verify(x => x.Map<ReadUserPostDTO>(It.IsAny<Blog.Domain.Entities.UserPost>()), Times.Never);
        Mapper.Verify(x => x.Map<Blog.Domain.Entities.UserPost>(updateRequest), Times.Once);
    }
    
}