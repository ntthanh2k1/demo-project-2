namespace Demo.Project2.Models
{
    public class Review : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public string? Username { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}
