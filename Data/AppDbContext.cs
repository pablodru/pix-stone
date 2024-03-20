using Microsoft.EntityFrameworkCore;
using Pix.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pix.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Bank>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Key>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Account>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Accounts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Bank>()
                .HasMany(b => b.Accounts)
                .WithOne(a => a.Bank)
                .HasForeignKey(a => a.BankId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Keys)
                .WithOne(k => k.Account)
                .HasForeignKey(k => k.AccountId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Payments)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId);

            modelBuilder.Entity<Key>()
                .HasMany(k => k.Payments)
                .WithOne(p => p.Key)
                .HasForeignKey(p => p.KeyId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.CPF)
                .HasDatabaseName("IX_User_CPF");
            
            modelBuilder.Entity<Bank>()
                .HasIndex(b => b.Token)
                .HasDatabaseName("IX_Bank_Token");

            modelBuilder.Entity<Key>()
                .HasIndex(k => new { k.Type, k.Value})
                .HasDatabaseName("IX_Key_Type_Value");

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified))
                .ToList();

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = now;
                }
                ((BaseEntity)entity.Entity).UpdatedAt = now;
            }
        }
    }
}
