namespace Demo.Project2.Models
{
    public class Order : BaseModel
    {
        public Guid UserId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
    }

    public enum OrderStatus
    {
        Processing = 0,
        Completed = 1,
        Cancelled = 2
    }
}