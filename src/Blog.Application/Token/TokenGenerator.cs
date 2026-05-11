using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public interface ITokenGenerator
{
    Task<string> CreateToken(User user);
}
public class TokenGenerator(IConfiguration configuration,UserManager<User> roleManager):ITokenGenerator
{
    public async Task<string> CreateToken(User user)
    {
        if (user == null)
        {
            throw new Exception("user not found");
        }

        var role =await roleManager.GetRolesAsync(user);
        // var listClaims=await roleManager.GetClaimsAsync(user);
        var listClaims = new List<Claim>(); 
        listClaims.Add(new Claim(ClaimTypes.Email,user.Email));
        listClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        listClaims.Add(new Claim(ClaimTypes.Name,user.UserName));
        foreach (var roleName in role)
        {
            listClaims.Add(new Claim(ClaimTypes.Role,roleName));   
        }
        var token = new JwtSecurityToken(signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
            SecurityAlgorithms.HmacSha256Signature), claims: listClaims, expires: DateTime.UtcNow.AddMinutes(20));
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}