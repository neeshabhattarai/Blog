using Microsoft.AspNetCore.Authorization;

namespace Blog.Infastructure.Authorization;

public class EmailRequiredRequirement(string email):IAuthorizationRequirement
{
   public string Email { get; set; } = email;
}