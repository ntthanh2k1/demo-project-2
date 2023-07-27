namespace Demo.Project2.Models
{
    public partial class Slide : BaseModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}