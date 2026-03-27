using DataQuality.Models;

namespace DataQuality.Services
{
    public class Accuracy
    {
        public void CheckAccuracy(List<Order> orders)
        {
            Console.WriteLine("--- Kvalitetskontroll: Rimlighetskontroll (Orders) ---");

            // Vi sätter dagens datum som referens för att hitta framtida ordrar
            DateTime today = DateTime.Now;

            foreach (var order in orders)
            {
                // 1. Kontrollera negativa belopp
                if (order.Amount < 0)
                {
                    Console.WriteLine($"LOGISKT FEL: Order {order.OrderId} har ett negativt belopp ({order.Amount}). Är detta en retur?");
                }

                // 2. Kontrollera framtida datum
                // Vi försöker parsa textsträngen från CSV-filen till ett riktigt datum
                if (DateTime.TryParse(order.OrderDate, out DateTime parsedDate))
                {
                    if (parsedDate > today)
                    {
                        Console.WriteLine($"LOGISKT FEL: Order {order.OrderId} har ett datum i framtiden ({order.OrderDate})!");
                    }
                }
            }

            Console.WriteLine("----------------------------------------------------------\n");
        }
    }
}
