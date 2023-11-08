using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Personal.Entities;
using Personal.Models;

namespace Personal.DataContext
{
    public class WalletContext: DbContext{
        public WalletContext(DbContextOptions<WalletContext> options):base(options){
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            _ = modelBuilder
                .Entity<Wallet>()
                .Property(d => d.Type)
                .HasConversion(new EnumToStringConverter<Personal.Models.Type>());
            }

        public override int SaveChanges()
            {
                AddTimestamps();
                return base.SaveChanges();
            }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
            {
                AddTimestamps();
                return base.SaveChangesAsync();
            }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

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
