using Demo.Project2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Demo.Project2.Context
{
    public class DemoProject2DbContext : DbContext
    {
        public DemoProject2DbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}
