namespace StockmngAPI.Models
{
    public class OrderCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<OrderItemType> Items { get; set; }
    }
}
