using DataQuality.Models;

namespace DataQuality.Services
{
    public class ConsistencyCountry
    {
        public void CheckConsistency(List<Customer> customers)
        {
            Console.WriteLine("--- Kvalitetskontroll: Kontrollerar konsistens (Land) ---");

            // 1. Definiera vad som är vårt "Master-värde" (Standard)
            string standardCountry = "Sweden";

            // 2. Skapa en lista på kända varianter som vi vill flagga
            var variants = new List<string> { "SE", "Sverige", "Svedala" };

            foreach (var customer in customers)
            {
                // Vi kollar om värdet finns i vår lista över "felaktiga" varianter
                // Vi använder ToUpper() för att inte bry oss om stora/små bokstäver
                if (variants.Contains(customer.Country))
                {
                    Console.WriteLine($"INKONSISTENS: Kund {customer.CustomerId} har '{customer.Country}'. Borde ändras till '{standardCountry}'.");
                }
            }

            Console.WriteLine("----------------------------------------------------------\n");
        }
    }
}
