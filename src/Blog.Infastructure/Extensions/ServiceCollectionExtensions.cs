using Blog.Infastructure.Data;
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
        services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<BlogDbContext>().AddDefaultTokenProviders();
    }
}