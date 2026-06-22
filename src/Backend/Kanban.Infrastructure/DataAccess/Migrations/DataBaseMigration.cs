using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kanban.Infrastructure.DataAccess.Migrations;

public static class DataBaseMigration
{
    public static async Task MigrateDatabase(IServiceProvider serviceProvider)  
    {        
        using KanbanDbContext dbContext = serviceProvider.GetRequiredService<KanbanDbContext>();  
        await dbContext.Database.MigrateAsync();  
    }
}