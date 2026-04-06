using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Application.Token;

public static class UserContext
{
    public static string CreateTokenofUser(HttpContextAccessor accessor, IConfiguration configuration)
    {
        var user = accessor.HttpContext.User;
        if (user == null)
        {
            return "user not found";
        }

        var identity = new Dictionary<string,object>();
        foreach (var claim in user.Claims)
        {
            identity.Add(claim.Type, claim.Value);
        }

        var token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            SigningCredentials =
                new SigningCredentials(configuration.GetValue<SecurityKey>("JWT:Key"),SecurityAlgorithms.HmacSha256),
            Expires = new DateTime().AddMinutes(20),
            Claims = identity
            
        });
        return token;
    }
    
}