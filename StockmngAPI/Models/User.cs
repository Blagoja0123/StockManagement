using System.Reflection.Metadata;

namespace StockmngAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int CartId { get; set; }
        public OrderCart OrderCart { get; set; }
        public List<Order> Orders { get; set; }
    }
}
