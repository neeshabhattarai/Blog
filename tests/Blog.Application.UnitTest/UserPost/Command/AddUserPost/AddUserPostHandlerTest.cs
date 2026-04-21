using AutoMapper;
using Blog.Application.UserPost.Command.AddUserPost;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;


namespace Test.UserPost.Command;
[TestFixture]
public class AddUserPostHandlerTest
{
    public Mock<IMapper> MapperMock;
    public Mock<IConfiguration> ConfigurationMock;
    public Mock<IUserPost> UserPostMock;
    public Blog.Domain.Entities.UserPost userPost;
    public AddUserPostCommand Command;
    [SetUp]
    public void AddUserPostHandlerTes()
    {
        MapperMock = new Mock<IMapper>();
        ConfigurationMock = new Mock<IConfiguration>();
        UserPostMock = new Mock<IUserPost>();
         userPost = new Blog.Domain.Entities.UserPost
        {

            PostId= "12334"
        };
             Command= new AddUserPostCommand
        {
            PostTitle = "Test",
            UserId = "1334",
        };
    }

    [Test]
    public async Task AddUserPostHandler_ShouldReturnSuccess()
    {
        
        UserPostMock.Setup(u => u.AddPost(userPost)).ReturnsAsync(userPost.PostId);
        MapperMock.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(Command)).Returns(userPost);
        var handler=new AddUserPostHandler(UserPostMock.Object,MapperMock.Object);
        var result = await handler.Handle(Command, CancellationToken.None);
        Assert.AreEqual(result,userPost.PostId);
    }
    [Test]
    public async Task AddUserPostHandler_ShouldReturnNull()
    {
        UserPostMock.Setup(u => u.AddPost(userPost)).ReturnsAsync((string?)null);
        MapperMock.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(Command)).Returns(userPost);
        var handler=new AddUserPostHandler(UserPostMock.Object,MapperMock.Object);
        var result = await handler.Handle(Command, CancellationToken.None);
        Assert.That(result,Is.Null);
    }
    [Test]
    public async Task AddUserPostHandler_ShouldReturnFailure_WhenDbConnectionFailure()
    {
        UserPostMock.Setup(u => u.AddPost(userPost)).ThrowsAsync(new Exception("Db connection failure"));
        MapperMock.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(Command)).Returns(userPost);
        var handler=new AddUserPostHandler(UserPostMock.Object,MapperMock.Object);
        var result =Assert.ThrowsAsync<Exception>(async () => await handler.Handle(Command, CancellationToken.None));
        Assert.AreEqual(result.Message,"Db connection failure");
    }

    [Test]
    public async Task ShouldReturnException_WhenMapperNotAssigned()
    {
        var handler = new AddUserPostHandler(UserPostMock.Object,MapperMock.Object);
        MapperMock.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(It.IsAny<AddUserPostCommand>())).Throws(new Exception("Mapper not assigned"));
        var result = Assert.ThrowsAsync<Exception>(async ()=>await handler.Handle(null, CancellationToken.None));
        MapperMock.Verify(x=>x.Map<Blog.Domain.Entities.UserPost>(It.IsAny<AddUserPostCommand>()), Times.Once);
        UserPostMock.Verify(x=>x.AddPost(It.IsAny<Blog.Domain.Entities.UserPost>()), Times.Never);
    }
}