using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Shoes;

public partial class ShoesContext : DbContext
{
    public ShoesContext()
    {
    }

    public ShoesContext(DbContextOptions<ShoesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickPoint> PickPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Shoes;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("Manufacturer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatus)
                .HasConstraintName("FK_Orders_OrderStatus");

            entity.HasOne(d => d.PickPointAddressNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickPointAddress)
                .HasConstraintName("FK_Orders_PickPoints");

            entity.HasOne(d => d.UserFullnameNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserFullname)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.ToTable("OrderProduct");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.HasOne(d => d.NumberOrderNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.NumberOrder)
                .HasConstraintName("FK_OrderProduct_Orders");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.Product)
                .HasConstraintName("FK_OrderProduct_Product");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.OrderStatus1)
                .HasMaxLength(255)
                .HasColumnName("OrderStatus");
        });

        modelBuilder.Entity<PickPoint>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Street).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Article).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Photo).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitMetric).HasMaxLength(255);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.ManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Manufacturer)
                .HasConstraintName("FK_Product_Manufacturer");

            entity.HasOne(d => d.SupplierNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Supplier)
                .HasConstraintName("FK_Product_Supplier");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Patronymic).HasMaxLength(255);
            entity.Property(e => e.Surname).HasMaxLength(255);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .HasConstraintName("FK_Users_UserRoles");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
