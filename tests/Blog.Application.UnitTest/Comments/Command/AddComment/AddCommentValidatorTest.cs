using Blog.Application.Comments.Command.AddComment;
using Xunit;

namespace Test.Comments.Command.AddComment;

public class AddCommentValidatorTest
{
    [Fact]
    public async Task AddComment_ShouldReturnSuccess()
    {
        var commentValidator = new AddCommentValidation();
        var commentInstance = new AddCommentCommand
        {
            Comment = "test",
            PostId = "23",
            UserId = "33"
        };
        var testResult=commentValidator.Validate(commentInstance);
        Assert.True(testResult.IsValid);
    }

    [Fact]
    public async Task AddComment_ShouldReturnFailure()
    {
        var commentValidator = new AddCommentValidation();
        var commentInstance = new AddCommentCommand();
        var testResult=commentValidator.Validate(commentInstance);
        Assert.False(testResult.IsValid);
    }
}