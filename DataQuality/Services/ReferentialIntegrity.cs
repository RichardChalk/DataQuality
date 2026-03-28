using DataQuality.Models;

namespace DataQuality.Services
{
    public class ReferentialIntegrity
    {
        public void CheckReferentialIntegrity(List<Customer> customers, List<Order> orders)
        {
            // 1. Skapa en snabb-lista (HashSet) med alla CustomerUniqueId som faktiskt finns i kundlistan
            List<int> validCustomerIds = customers.Select(c => c.CustomerId).ToList();

            Console.WriteLine("--- Kvalitetskontroll: Kontrollerar referensintegritet ---");

            int orphanCount = 0;

            foreach (var order in orders)
            {
                // 2. Kontrollera om orderns CustomerUniqueId finns i vår lista över giltiga kunder
                if (!validCustomerIds.Contains(order.CustomerId))
                {
                    // Om ID:t saknas är detta en "föräldralös" (orphaned) order
                    Console.WriteLine($"VARNING: Order {order.OrderId} refererar till CustomerUniqueId {order.CustomerId} som INTE existerar!");
                    orphanCount++;
                }
            }

            if (orphanCount == 0)
            {
                Console.WriteLine("All referensintegritet är ok: Alla ordrar tillhör en existerande kund.");
            }
            else
            {
                Console.WriteLine($"Kontroll klar: Hittade {orphanCount} ordrar med saknade kundkopplingar.");
            }
            Console.WriteLine("----------------------------------------------------------\n");
        }
    }
}
