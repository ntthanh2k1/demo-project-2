namespace Demo.Project2.Models
{
    public partial class Category
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; } = new HashSet<Category>();
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}