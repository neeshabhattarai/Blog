using Arch.EntityFrameworkCore.UnitOfWork;
using Blog.Infastructure.Data;

namespace Blog.Infastructure.Repository;

public interface IUnitofWorkSave
{
    Task<bool> SaveChangesAsync();
}

public class UnitofWorkSave(BlogDbContext context):IUnitofWorkSave
{
    
    public async Task<bool> SaveChangesAsync()
    {
        await context.SaveChangesAsync();
        return true;
    }
}