using DataQuality.Models;
using Microsoft.EntityFrameworkCore;

namespace DataQuality.Data
{
    public class DataInitializer
    {
        public void MigrateDatabase(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
            dbContext.SaveChanges();
        }

        public void PopulateDatabase(AppDbContext dbContext, List<Customer> customersCleaned, List<Order> ordersCleaned)
        {
            if (!dbContext.Customers.Any())
            {
                Console.WriteLine("Databasen är tom. Påbörjar länkning och import...");

                // 1. Unika kunder
                var uniqueCustomers = customersCleaned.DistinctBy(c => c.CustomerId).ToList();

                // 2. Länka ordrar och rensa bort de som saknar kund (Referensintegritet)
                foreach (var order in ordersCleaned)
                {
                    var matchingCustomer = uniqueCustomers.FirstOrDefault(c => c.CustomerId == order.CustomerId);

                    if (matchingCustomer != null)
                    {
                        order.Customer = matchingCustomer;
                        // Vi nollställer CustomerId-int-värdet så att EF använder objekt-kopplingen istället
                        order.CustomerId = 0;
                    }
                }

                // Filtrera ut endast de ordrar som faktiskt fick en matchande kund
                var validOrders = ordersCleaned.Where(o => o.Customer != null).ToList();

                // 3. Spara till databasen
                dbContext.Customers.AddRange(uniqueCustomers);
                dbContext.Orders.AddRange(validOrders);

                dbContext.SaveChanges();
                Console.WriteLine($"Succé! Importerade {uniqueCustomers.Count} kunder och {validOrders.Count} ordrar.");

                // Bonus: Berätta om vi kastade bort några trasiga ordrar
                int orphanedOrders = ordersCleaned.Count - validOrders.Count;
                if (orphanedOrders > 0)
                {
                    Console.WriteLine($"OBS: {orphanedOrders} ordrar kastades p.g.a. saknad kundkoppling.");
                }
            }
        }
    }
}
