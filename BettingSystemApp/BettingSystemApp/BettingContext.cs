using System.Data.Entity;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public class BettingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Journal> Journals { get; set; }

        public BettingContext() : base("name=BettingSystem") { }
    }
}
