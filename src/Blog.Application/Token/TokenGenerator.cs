using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public interface ITokenGenerator
{
    object CreateToken(IdentityUser user);
}
public class TokenGenerator(IConfiguration configuration):ITokenGenerator
{
    public object CreateToken(IdentityUser user)
    {
        if (user == null)
        {
            throw new Exception("user not found");
        }
        var listClaims = new List<Claim>(); 
        listClaims.Add(new Claim(ClaimTypes.Email,user.Email));
        listClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        listClaims.Add(new Claim(ClaimTypes.Name,user.UserName));
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