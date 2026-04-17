using Blog.Application.UserPost.Command.AddUserPost;
using Moq;
using NUnit.Framework;

namespace Test.UserPost.Command;
[TestFixture]
public class AddUserPostValidationTest
{
    private AddUserPostValidation validation;
    [SetUp]
    public void AddUserValidation()
    {
         validation = new ();
    }
    [Test]
    public async Task AddUserPost_ReturnSuccess()
    {
        var addUser = new AddUserPostCommand
        {
            PostTitle = "First Post",
            UserId = "12"
        };
       
       var result= validation.Validate(addUser);
       Assert.That(result.IsValid);

    }
    [Test]
    public async Task AddUserPost_ReturnFailure()
    {
        var newAddPost = new AddUserPostCommand();
        var result= validation.Validate(newAddPost);
        Assert.False(result.IsValid);
        Assert.AreEqual("PostTitle", result.Errors[0].PropertyName);
    }
}