namespace DataQuality.Models
{
    public class Customer
    {
        public int CustomerId { get; set; } // Blir automatiskt Primary Key
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
