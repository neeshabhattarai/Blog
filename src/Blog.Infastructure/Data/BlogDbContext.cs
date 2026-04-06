using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infastructure.Data;

public class BlogDbContext:IdentityDbContext
{
    public  BlogDbContext(DbContextOptions options):base(options)
    {
    }
}