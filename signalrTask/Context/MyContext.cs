
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using signalrTask.Models;

namespace signalrTask.Context
{
    public class MyContext : IdentityDbContext<Employee>
    {
        public MyContext()
        {

        }
        public MyContext(DbContextOptions<MyContext> op) : base(op)
        {

        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserGroup>().HasKey(ug => new { ug.Username, ug.Groupname });
        }
    }
}
