using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniCruiter.Models.Identity;

namespace UniCruiter.Data
{
    public class UniCruiterContext : IdentityDbContext<ApplicationUser>
    {
        public UniCruiterContext (DbContextOptions<UniCruiterContext> options)
            : base(options)
        {
        }

        public DbSet<UniCruiter.Models.Student> Student { get; set; }
    }
}
