using System.Security.Claims;
using System.Text;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public interface ITokenGenerator
{
    Task<object> CreateToken(User user);
}
public class TokenGenerator(IConfiguration configuration,UserManager<User> roleManager):ITokenGenerator
{
    public async Task<object> CreateToken(User user)
    {
        if (user == null)
        {
            throw new Exception("user not found");
        }

        var role =await roleManager.GetRolesAsync(user);
        var listClaims=await roleManager.GetClaimsAsync(user);
        // var listClaims = new List<Claim>(); 
        // listClaims.Add(new Claim(ClaimTypes.Email,user.Email));
        // listClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        // listClaims.Add(new Claim(ClaimTypes.Name,user.UserName));
        foreach (var roleName in role)
        {
            listClaims.Add(new Claim(ClaimTypes.Role,roleName));   
        }
        // listClaims.Add(new Claim("IsAdmin",true.ToString()));
        var claimsDictionary=new Dictionary<string,object>();
        foreach (var claim in listClaims)
        {
        claimsDictionary.Add(claim.Type, claim.Value);    
        }
        var expires = DateTime.UtcNow.AddMinutes(20);
        var token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor()
        {
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    SecurityAlgorithms.HmacSha256Signature),
            Expires = expires,
            Claims = claimsDictionary

        });
        return new
        {
            token,
            expires
        };

    }
}