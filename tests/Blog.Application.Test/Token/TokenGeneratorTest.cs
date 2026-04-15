using System.Security.Claims;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Test.Token;

public class TokenGeneratorTest
{
    [Fact]
    public async Task GenerateToken_ShouldReturnSuccess()
    {
        var configuration=new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT:Key"]).Returns("TestKeykjlkahkjhfjkdkjfhal;fhjkjh");
        var store=new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(store.Object,null,null,null,null,null,null,null,null);
        
        TokenGenerator tokenGenerator = new TokenGenerator(configuration.Object, userManager.Object);
        var user=new User
        {
            Id="1334¢54",
            Email = "test@gmail.com",
            UserName = "test",
        };
        userManager.Setup(x => x.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
        userManager
            .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>());

      var token= await tokenGenerator.CreateToken(user);
        Assert.NotNull(token);
        
    }
    
}