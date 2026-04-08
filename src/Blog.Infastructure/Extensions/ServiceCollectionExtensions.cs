using Blog.Domain.Entities;
using Blog.Domain.Repository;
using Blog.Infastructure.Data;
using Blog.Infastructure.Repository;
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
    }
}