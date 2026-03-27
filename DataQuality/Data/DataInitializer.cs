using Microsoft.EntityFrameworkCore;

namespace DataQuality.Data
{
    public class DataInitializer
    {
        public void MigrateAndSeed(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
            // SeedData(dbContext);
            dbContext.SaveChanges();
        }
    }
}
