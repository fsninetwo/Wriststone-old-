using System.Data.Entity;
using Wriststone.Models.Table;

namespace Wriststone.Models
{
    public class WriststoneContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }
        //public DbSet<Payment> Payments { get; set; }
        //public DbSet<Tag> Tags { get; set; }
        //public DbSet<ProductTag> ProductTags { get; set; }
        //public DbSet<Status> Statuses { get; set; }

        public WriststoneContext() : base("WriststoneContext")
        {
            Database.SetInitializer<WriststoneContext>(null);
        }
    }
}