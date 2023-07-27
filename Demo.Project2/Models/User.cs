namespace Demo.Project2.Models
{
    public partial class User : BaseModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}