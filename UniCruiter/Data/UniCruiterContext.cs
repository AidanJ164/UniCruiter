using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniCruiter.Models;
using UniCruiter.Models.Identity;

namespace UniCruiter.Data
{
    public class UniCruiterContext : IdentityDbContext<ApplicationUser>
    {
        public UniCruiterContext(DbContextOptions<UniCruiterContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
