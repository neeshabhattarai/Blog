using Blog.Application.UserPost.Command.AddUserPost;
using Moq;
using Xunit;

namespace Test.UserPost.Command;

public class AddUserPostValidationTest
{
    [Fact]
    public async Task AddUserPost_ReturnSuccess()
    {
        var addUser = new AddUserPostCommand
        {
            PostTitle = "First Post",
            UserId = "12"
        };
        var validation = new AddUserPostValidation();
       var result= validation.Validate(addUser);
       Assert.True(result.IsValid);

    }
    [Fact]
    public async Task AddUserPost_ReturnFailure()
    {
        var validation = new AddUserPostValidation();
        var newAddPost = new AddUserPostCommand();
        var result= validation.Validate(newAddPost);
        Assert.False(result.IsValid);
        Assert.Equal("PostTitle", result.Errors[0].PropertyName);
    }
}