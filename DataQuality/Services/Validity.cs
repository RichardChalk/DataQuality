using DataQuality.Models;

namespace DataQuality.Services
{
    public class Validity
    {
        public void CheckValidity(List<Customer> customers, List<Order> orders)
        {
            Console.WriteLine("--- Kvalitetskontroll: Kontrollerar format på e-post ---");

            foreach (var customer in customers)
            {
                // Vi hoppar över de som redan är tomma (det fångade vi i förra kontrollen)
                if (!string.IsNullOrWhiteSpace(customer.Email))
                {
                    // En mycket enkel kontroll: Innehåller den @ och . ?
                    if (!customer.Email.Contains("@") || !customer.Email.Contains("."))
                    {
                        Console.WriteLine($"FORMATFEL: Kund {customer.CustomerId} har en ogiltig e-post: '{customer.Email}'");
                    }
                }
            }

            Console.WriteLine("----------------------------------------------------------\n");
        }
    }
}
