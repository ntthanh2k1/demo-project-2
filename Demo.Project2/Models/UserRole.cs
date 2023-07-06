using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Demo.Project2.Models
{
    public partial class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public bool Status { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
