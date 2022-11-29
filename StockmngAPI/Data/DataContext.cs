using Microsoft.EntityFrameworkCore;
using StockmngAPI.Models;

namespace StockmngAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {     
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCart> OrderCarts { get; set; }
        public DbSet<OrderItemType> OrderItemTypes { get; set; }
    }
}
