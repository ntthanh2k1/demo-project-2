using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Demo.Project2.Models
{
    [Table("UserRole")]
    public partial class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
