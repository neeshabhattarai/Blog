using Blog.Application.Comments.Command.AddComment;
using NUnit.Framework;

namespace Test.Comments.Command.AddComment;
[TestFixture]
public class AddCommentValidatorTest
{
    [Test]
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
        Assert.That(testResult.IsValid);
    }

    [Test]
    public async Task AddComment_ShouldReturnFailure()
    {
        var commentValidator = new AddCommentValidation();
        var commentInstance = new AddCommentCommand();
        var testResult=commentValidator.Validate(commentInstance);
        Assert.That(testResult.IsValid,Is.False);
    }
}