using DataQuality.Models;

namespace DataQuality.Services
{
    public class LoadInitialData
    {
        public List<Customer> ReadCustomers(string filePath)
        {
            var customers = new List<Customer>();
            var lines = File.ReadAllLines(filePath);

            // Startar på 1 för att hoppa över rubrikerna (index 0)
            for (int i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split('\t');
                if (columns.Length >= 7)
                {
                    customers.Add(new Customer
                    {
                        CustomerId = int.Parse(columns[0]),
                        FirstName = columns[1],
                        LastName = columns[2],
                        Email = columns[3],
                        Phone = columns[4],
                        RegDate = columns[5],
                        Country = columns[6]
                    });
                }
            }
            return customers;
        }

        public List<Order> ReadOrders(string filePath)
        {
            var orders = new List<Order>();
            var lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split('\t');
                if (columns.Length >= 8)
                {
                    orders.Add(new Order
                    {
                        OrderId = int.Parse(columns[0]),
                        CustomerId = int.Parse(columns[1]),
                        OrderDate = columns[2],
                        Amount = decimal.Parse(columns[3], System.Globalization.CultureInfo.InvariantCulture),
                        Currency = columns[4],
                        Status = columns[5],
                        ShippingAddress = columns[6],
                        City = columns[7]
                    });
                }
            }
            return orders;
        }
    }
}
