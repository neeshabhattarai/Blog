using Blog.Infastructure.Data;
using Blog.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Blog.Api.IntegrationTest.Services;

public class CustomWebApplicationFactory<Top>:WebApplicationFactory<Top> where Top : class 
{
    public Mock<IEmailService> EmailService { get; set; }
   override 
    protected void ConfigureWebHost(IWebHostBuilder builder)
   {
       builder.ConfigureServices(services =>
       {
           var descriptor=services.SingleOrDefault(x=>x.ServiceType == typeof(DbContextOptions<BlogDbContext>));
            if(descriptor!=null)
                services.Remove(descriptor);
           services.AddDbContext<BlogDbContext>(options =>
           {
               options.UseNpgsql("Host=localhost;Port=5430;Database=BlogDb_Test;Username=postgres;Password=admin;");
           });
           EmailService = new Mock<IEmailService>();
           services.AddSingleton(EmailService.Object);
           var sp = services.BuildServiceProvider();
           using var scope = sp.CreateScope();
           var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
           db.Database.EnsureDeleted();
           db.Database.EnsureCreated();
       });
   }
}