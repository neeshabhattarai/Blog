using Blog.Infastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blog.Api.IntegrationTest.Services;

public class CustomWebApplicationFactory<Top>:WebApplicationFactory<Top> where Top : class 
{
   override 
    protected void ConfigureWebHost(IWebHostBuilder builder)
   {
       builder.ConfigureServices(services =>
       {
           services.AddDbContext<BlogDbContext>(options =>
           {

               options.UseInMemoryDatabase("TestDatabase");
           });
       });
   }
}