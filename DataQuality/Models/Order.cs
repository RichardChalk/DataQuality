using System.ComponentModel.DataAnnotations;

namespace DataQuality.Models
{
    public class Order
    {
        [Key]
        public int SQLId { get; set; }// Blir automatiskt Primary Key i SQL
        public int OrderId { get; set; }
        public int CustomerId { get; set; } // Foreign Key
        public string OrderDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }

        // Navigeringsegenskap tillbaka till kunden
        public Customer Customer { get; set; }
    }
}
