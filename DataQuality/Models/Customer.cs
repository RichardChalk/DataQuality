using System.ComponentModel.DataAnnotations;

namespace DataQuality.Models
{
    public class Customer
    {
        [Key]
        public int SQLId { get; set; }// Blir automatiskt Primary Key i SQL
        public int CustomerId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RegDate { get; set; }
        public string Country { get; set; }

        // Relation: En kund kan ha många ordrar
        public List<Order> Orders { get; set; }
    }
}
