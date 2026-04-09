using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Infastructure.Authorization;

public class EmailRequirementHandler:AuthorizationHandler<EmailRequiredRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailRequiredRequirement requirement)
    {
        var user = context.User.FindFirst(ClaimTypes.Email);
        if (requirement.Email == user.Value)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        context.Fail();
        return Task.CompletedTask;
    }
}