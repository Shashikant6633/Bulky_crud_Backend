using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Bulky_crud.Models
{
    public class CategoryContext : DbContext
    {
        public CategoryContext(DbContextOptions<CategoryContext> options) : base(options)
        {
        
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> CartDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
