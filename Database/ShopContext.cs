using Microsoft.EntityFrameworkCore;
using ProductCrud.Model;

namespace ProductCrud.Database;

public class ShopContext : DbContext
{
    public DbSet<Product> Product { get; set; }
    public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }
}