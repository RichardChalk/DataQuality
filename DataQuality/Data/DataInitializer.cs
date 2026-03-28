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
                Console.WriteLine("Databasen är tom. Påbörjar länkning av data...");

                // 1. Tvätta kunddatan (ta bort dubbletter baserat på CSV-ID)
                var uniqueCustomers = customersCleaned.DistinctBy(c => c.CustomerId).ToList();

                // 2. Länka ihop ordrar med rätt kund-objekt
                foreach (var order in ordersCleaned)
                {
                    // Hitta kunden i vår lista som har samma ID som i order-filen
                    var matchingCustomer = uniqueCustomers.FirstOrDefault(c => c.CustomerId == order.CustomerId);

                    if (matchingCustomer != null)
                    {
                        // Genom att sätta objekt-referensen 'Customer' istället för 'CustomerId' (int)
                        // så sköter EF all ID-matchning åt oss i bakgrunden.
                        order.Customer = matchingCustomer;
                    }
                }

                // 3. Lägg till kunderna. Eftersom ordrarna nu är länkade till kunderna 
                // i minnet, kommer EF att förstå att de ska sparas tillsammans.
                dbContext.Customers.AddRange(uniqueCustomers);

                // Vi behöver inte köra AddRange på ordersCleaned separat, EF hittar dem via navigeringen.

                dbContext.SaveChanges();
                Console.WriteLine($"Klart! Importerade {uniqueCustomers.Count} kunder och deras tillhörande ordrar.");
            }
        }
    }
}
