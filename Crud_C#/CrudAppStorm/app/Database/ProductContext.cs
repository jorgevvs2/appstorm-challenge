using CrudAppStorm.app.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudAppStorm.app.Database
{
    public class ProductContext : DbContext
    {
        const string strCon = @"Data Source=(LocalDb)\MSSQLLocalDB; Initial Catalog=Store;Integrated Security=SSPI;";

        public ProductContext(DbContextOptions<ProductContext> options) : base(options) {
            Database.EnsureCreated();
        }

        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Product>();
            builder.Property(c => c.Id)
                .IsRequired(true);

            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(c => c.Price)
                   .IsRequired(true);

            builder.Property(c => c.Stock)
                .HasMaxLength(300)
                .IsRequired(true);
        }

    }
}
