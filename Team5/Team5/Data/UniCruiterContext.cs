using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniCruiter.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
