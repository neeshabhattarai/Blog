using System.Security.Claims;
using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Blog.Infastructure.Authorization;
using Blog.Infastructure.Data;
using Blog.Infastructure.Repository;
using Blog.Infastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfastructureServices(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddDbContext<BlogDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<ICommentText, CommentRepository>();
        services.AddScoped<IUserPost, UserPostRepository>();
        services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<BlogDbContext>().AddDefaultTokenProviders();
        services.AddAuthorizationBuilder().AddPolicy("IsAdmin",
                builder => builder.RequireClaim(ClaimTypes.Role, Role.Admin))
            .AddPolicy("IsUser", builder => builder.RequireClaim(ClaimTypes.Role, Role.User))
            .AddPolicy("IsManager", builder => builder.RequireClaim(ClaimTypes.Role, Role.Manager))
            .AddPolicy("IsAuthor", builder => builder.AddRequirements(new EmailRequiredRequirement("test@gmail.com")))
            .AddPolicy("IsAdminOrUser",builder=>builder.RequireAssertion(context=>context.User.IsInRole(Role.Admin)||context.User.IsInRole(Role.User)));
        services.AddScoped<IAuthorizationHandler, EmailRequirementHandler>();
    }
}