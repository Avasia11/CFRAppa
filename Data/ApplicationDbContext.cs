using Microsoft.EntityFrameworkCore;
using CFRApp.Models;

namespace CFRApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<CFRApp.Models.Route> Routes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Delay> Delays { get; set; }
        public DbSet<Payment> Payments { get; set; }

        




    }



}

