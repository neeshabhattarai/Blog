using AutoMapper;
using Blog.Application.UserPost.Command.AddUserPost;
using Blog.Domain.Repository;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Test.UserPost.Command;

public class AddUserPostHandlerTest
{
    public Mock<IMapper> MapperMock;
    public Mock<IConfiguration> ConfigurationMock;
    public Mock<IUserPost> UserPostMock;
    public AddUserPostHandlerTest()
    {
        MapperMock = new Mock<IMapper>();
        ConfigurationMock = new Mock<IConfiguration>();
        UserPostMock = new Mock<IUserPost>();
    }

    [Fact]
    public async Task AddUserPostHandler_ShouldReturnSuccess()
    {
        var userPost = new Blog.Domain.Entities.UserPost
        {
            PostTitle = "Test",
            UserId = "1334",
            PostId= "12334"
        };
        UserPostMock.Setup(u => u.AddPost(userPost)).ReturnsAsync(userPost.PostId);
        MapperMock.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(It.IsAny<AddUserPostCommand>())).Returns(userPost);
        var handler=new AddUserPostHandler(UserPostMock.Object,MapperMock.Object);
        var result = await handler.Handle(It.IsAny<AddUserPostCommand>(), CancellationToken.None);
        Assert.Equal(result,userPost.PostId);
    }
    [Fact]
    public async Task AddUserPostHandler_ShouldReturnNull()
    {
        var userPost = It.IsAny<Blog.Domain.Entities.UserPost>();
        UserPostMock.Setup(u => u.AddPost(userPost)).ReturnsAsync((string?)null);
        MapperMock.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(It.IsAny<AddUserPostCommand>())).Returns(userPost);
        var handler=new AddUserPostHandler(UserPostMock.Object,MapperMock.Object);
        var result = await handler.Handle(It.IsAny<AddUserPostCommand>(), CancellationToken.None);
        Assert.Equal(result,null);
    }
    [Fact]
    public async Task AddUserPostHandler_ShouldReturnFailure()
    {
        var userPost = It.IsAny<Blog.Domain.Entities.UserPost>();
        UserPostMock.Setup(u => u.AddPost(userPost)).ThrowsAsync(new Exception("Db connection failure"));
        MapperMock.Setup(x=>x.Map<Blog.Domain.Entities.UserPost>(It.IsAny<AddUserPostCommand>())).Returns(userPost);
        var handler=new AddUserPostHandler(UserPostMock.Object,MapperMock.Object);
        var result =await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(It.IsAny<AddUserPostCommand>(), CancellationToken.None));
        Assert.Equal(result.Message,"Db connection failure");
    }
}