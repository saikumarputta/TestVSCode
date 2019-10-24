using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestVSCode.Models;

namespace TodoApi.Models
{
    public class TodoContext : IdentityDbContext<IdentityUser>
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
         base.OnModelCreating(builder);

         builder.Entity<IdentityRole>().HasData(
             new {Id = "1",Name = "Admin",NormalizedName = "ADMIN"},
             new {Id = "2",Name = "Customer",NormalizedName = "CUSTOMER"}
         );
        }
    }
}