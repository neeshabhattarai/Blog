using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Blog.Application.User.DTO;
using Blog.Domain.Entities;
using Blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace Blog.Controller;
[ApiController]
[Route("[action]")]
public class UserController(UserManager<User> userManager,RoleManager<IdentityRole> roleManager,SignInManager<User> signInManager,ITokenGenerator tokenGenerator,IEmailService emailService,IConfiguration configuration):ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddRole(string roleName)
    {
        var user = User;
        var identityUser=await userManager.GetUserAsync(User);
        var roles=new List<string>{"Admin".ToUpper(),"User".ToUpper(),"Manager".ToUpper()};
        if (!roleManager.Roles.Any())
        {
            foreach (var role in roles)
            {
              await  roleManager.CreateAsync(new IdentityRole(role));

            }
        }

        var result = roles.Contains(roleName);
        if (result == false)
        {
            return BadRequest("Role not found");
        }
      var resultRole= await userManager.AddToRoleAsync(identityUser,roleName);
      if (resultRole.Succeeded)
      {
          return Ok("User role assigned successfully");
      }
      return BadRequest(resultRole.Errors);
    }
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] AddUser user)
    {
        var identityUser = new User
        {
            UserName = user.Email,
            Email = user.Email
        };
        var result =await userManager.CreateAsync(identityUser, user.Password);
        if (result.Succeeded)
        {
            await userManager.ResetAuthenticatorKeyAsync(identityUser);
           await userManager.SetTwoFactorEnabledAsync(identityUser, true);
           return Ok("User created successfully");
        }
        return BadRequest(result.Errors);
        
    }

    [HttpPost]
    public async Task<IActionResult> LoginWith2FA([FromBody] AddUser user)
    {
        var emailUser = await userManager.FindByEmailAsync(user.Email);
        if (emailUser == null)
        {
            return BadRequest("Invalid credentials");
        }

        var result = await signInManager.PasswordSignInAsync(emailUser, user.Password, false, false);
        if (result.Succeeded)
        {
            return Ok("User created successfully");
        }
        else
        {
            if (result.RequiresTwoFactor)
            {
                var token = await userManager.GenerateTwoFactorTokenAsync(emailUser, "Email");
                await emailService.SendEmailAsync(emailUser.Email, "TwoFactor", token);
                return Ok("Please check your email inbox");
            }
            
        }

        return BadRequest("Login Failed");
}
   
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Configure2FA(string token)
    {
        var user = User;
        var identityUser =
            await userManager.FindByEmailAsync(user.Claims.FirstOrDefault(c => ClaimTypes.Email == c.Type)?.Value);
        if (identityUser == null)
        {
            throw new Exception("User not authorized");
        }

        var result = await userManager.VerifyTwoFactorTokenAsync(identityUser, "Email", token);
        if (result)
        {
            await userManager.SetTwoFactorEnabledAsync(identityUser, true);
            return Ok("Thankyou for confirming");
        }
        return BadRequest("User not authorized");
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveUser()
    {
        var user = User;
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var identityUser=await userManager.FindByEmailAsync(user.FindFirst(ClaimTypes.Email)?.Value);
            await userManager.DeleteAsync(identityUser);
            return Ok("User deleted successfully");
    }
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GenerateQrCode()
    {
        var user=User;
        var identityUser =await userManager.GetUserAsync(user);
        if (identityUser == null)
        {
            return Unauthorized();
        }

        var token = await userManager.GetAuthenticatorKeyAsync(identityUser);
        if (token == null)
        {
             await userManager.ResetAuthenticatorKeyAsync(identityUser);
             token=await userManager.GetAuthenticatorKeyAsync(identityUser);

        }

        var qrCodeReader = new QRCodeGenerator();
        var qrCodeUrl = $"otpauth://totp/MyApp:{identityUser.Email}?secret={token}&issuer=MyApp&digits=6";
        var createQrCode = qrCodeReader.CreateQrCode(qrCodeUrl,QRCodeGenerator.ECCLevel.Q);
        var pngFormat=new PngByteQRCode(createQrCode);
        
       return File(pngFormat.GetGraphic(20),"image/png");
    }
[HttpPost]
[ProducesResponseType(StatusCodes.Status200OK) ]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GenerateToken([FromBody] AddUser user)
    {
        var users=await userManager.FindByEmailAsync(user.Email);
        if (users==null)
            return BadRequest("User not found");
        var check=await userManager.CheckPasswordAsync(users,user.Password);
        if (!check)
        {
            return Unauthorized();
        }

        var token =await tokenGenerator.CreateToken(users);
        return Ok(token);
    }
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthenticateWithQr(string code)
    {
        var user=User;
        var identityUser=await userManager.GetUserAsync(user);
        var result=await userManager.VerifyTwoFactorTokenAsync(identityUser, userManager.Options.Tokens.AuthenticatorTokenProvider,
            code);
        if (!result)
        {
            return BadRequest("Something went wrong,please try again");
        }
        return Ok("User authenticated successfully");
    }
    
}