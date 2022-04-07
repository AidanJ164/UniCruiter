using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UniCruiter.Data
{
    public class UniCruiterContext : IdentityDbContext
    {
        public UniCruiterContext (DbContextOptions<UniCruiterContext> options)
            : base(options)
        {
        }

        public DbSet<UniCruiter.Models.Student> Student { get; set; }
    }
}
