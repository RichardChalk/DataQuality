using DataQuality.Data;
using DataQuality.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataQuality
{
    public class Application
    {
        public void Run()
        {
            // Skapa databasen
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            using (var dbContext = new AppDbContext(options.Options))
            {
                var dataInitiaizer = new DataInitializer();
                dataInitiaizer.MigrateAndSeed(dbContext);
            }

            // Hämta Customers
            var loadCustomers = new LoadInitialData();
            
            var filepathCustomers = "..\\..\\..\\OriginalDataInExcel\\Customers.csv";
            var customers = loadCustomers.ReadCustomers(filepathCustomers);

            // Hämta Orders
            var filepathOrders = "..\\..\\..\\OriginalDataInExcel\\Orders.csv";
            var orders = loadCustomers.ReadOrders(filepathOrders);

            // Quality Checks

            // Kontrollera Id dubbletter i både tabeller
            new QualityChecks().CheckForDuplicateIds(customers, orders);
        }
    }
}
