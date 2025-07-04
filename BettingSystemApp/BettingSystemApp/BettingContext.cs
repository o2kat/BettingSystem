using System.Data.Entity;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public class BettingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<UserBet> UserBets { get; set; }

        public BettingContext() : base("name=BettingSystem") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Конфигурация для User
            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasPrecision(10, 2);

            // Конфигурация для Bet
            modelBuilder.Entity<Bet>()
                .Property(b => b.Team1Win)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Bet>()
                .Property(b => b.Team2Win)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Bet>()
                .Property(b => b.Draw)
                .HasPrecision(5, 2);

            // Конфигурация для UserBet
            modelBuilder.Entity<UserBet>()
                .Property(ub => ub.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UserBet>()
                .Property(ub => ub.Coefficient)
                .HasPrecision(5, 2);

            modelBuilder.Entity<UserBet>()
                .Property(ub => ub.TeamWin)
                .HasMaxLength(50);

            modelBuilder.Entity<UserBet>()
                .Property(ub => ub.Status)
                .HasMaxLength(20);

            // Связи
            modelBuilder.Entity<UserBet>()
                .HasRequired(ub => ub.User)
                .WithMany(u => u.UserBets)
                .HasForeignKey(ub => ub.UserID);

            modelBuilder.Entity<UserBet>()
                .HasRequired(ub => ub.Bet)
                .WithMany(b => b.UserBets)
                .HasForeignKey(ub => ub.BetID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
