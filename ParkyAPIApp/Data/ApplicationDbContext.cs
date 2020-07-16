using Microsoft.EntityFrameworkCore;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<NationalPark> NationalParks { get; set; }
    }
}
