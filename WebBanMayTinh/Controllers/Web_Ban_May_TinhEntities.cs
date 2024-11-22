using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebBanMayTinh.Models;

public partial class Web_Ban_May_TinhEntities : DbContext
{
    public Web_Ban_May_TinhEntities()
        : base("name=Web_Ban_May_TinhEntities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ProductDescription> ProductDescriptions { get; set; }
}