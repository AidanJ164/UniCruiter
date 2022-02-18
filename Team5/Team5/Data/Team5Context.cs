using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Team5.Models;

namespace Team5.Data
{
    public class Team5Context : DbContext
    {
        public Team5Context (DbContextOptions<Team5Context> options)
            : base(options)
        {
        }

        public DbSet<Team5.Models.Student> Student { get; set; }
    }
}
