namespace StockmngAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Recipiant { get; set; }
        public int OrderCartId { get; set; }
        public List<OrderItemType> Cart { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Status Status { get; set; } = Status.Pending;
    }

    public enum Status
    {
        Pending,
        Processing,
        Completed,
    }
}
