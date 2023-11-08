using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
}
}
