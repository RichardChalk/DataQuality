using DataQuality.Models;

namespace DataQuality.Services
{
    public class Completeness
    {
        public void CheckCompleteness(List<Customer> customers, List<Order> orders)
        {
            Console.WriteLine("--- Kvalitetskontroll: Letar efter saknade värden (Completeness) ---");

            // 1. Kontrollera Kunder (t.ex. E-post)
            foreach (var customer in customers)
            {
                if (string.IsNullOrWhiteSpace(customer.Email))
                {
                    Console.WriteLine($"VARNING: Kund {customer.CustomerId} ({customer.FirstName} {customer.LastName}) saknar e-postadress!");
                }
            }

            // 2. Kontrollera Ordrar (t.ex. Stad för leverans)
            foreach (var order in orders)
            {
                if (string.IsNullOrWhiteSpace(order.City))
                {
                    Console.WriteLine($"VARNING: Order {order.OrderId} saknar destinationsort (City)!");
                }
            }

            Console.WriteLine("----------------------------------------------------------\n");
            Console.WriteLine("----------------------------------------------------------\n");
        }
    }
}
