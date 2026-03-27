using DataQuality.Models;

namespace DataQuality.Services
{
    public class DuplicateChecksId
    {
        public void CheckForDuplicateIds(List<Customer> customers, List<Order> orders)
        {
            // 1. Skapa en tom lista för att hålla koll på de Customer ID:n vi redan sett
            List<int> seenIdsCustomers = new List<int>();

            Console.WriteLine("--- Kvalitetskontroll: Letar efter dubbletter Customers ---");

            // Kontrollera kunder först
            foreach (var customer in customers)
            {
                // Kontrollera om ID:t redan finns i vår "kom-ihåg-lista"
                if (seenIdsCustomers.Contains(customer.CustomerId))
                {
                    // Om det finns: Vi har hittat en dubblett!
                    Console.WriteLine($"FEL: Dubblett upptäckt! ID {customer.CustomerId} tillhör både {customer.FirstName} {customer.LastName} och någon annan.");
                }
                else
                {
                    // Om det inte finns: Lägg till ID:t i listan så vi minns det till nästa varv i loopen
                    seenIdsCustomers.Add(customer.CustomerId);
                }
            }

            // 2. Skapa en tom lista för att hålla koll på de Customer ID:n vi redan sett
            List<int> seenIdsOrders = new List<int>();

            Console.WriteLine("--- Kvalitetskontroll: Letar efter dubbletter Orders ---");

            // Kontrollera kunder först
            foreach (var order in orders)
            {
                // Kontrollera om ID:t redan finns i vår "kom-ihåg-lista"
                if (seenIdsCustomers.Contains(order.OrderId))
                {
                    // Om det finns: Vi har hittat en dubblett!
                    Console.WriteLine($"FEL: Dubblett upptäckt! ID {order.OrderId}");
                }
                else
                {
                    // Om det inte finns: Lägg till ID:t i listan så vi minns det till nästa varv i loopen
                    seenIdsCustomers.Add(order.OrderId);
                }
            }
            Console.WriteLine("--- Kontrollen klar ---\n");
        }
    }
}
