using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locerp.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();

    public DbSet<CustomerDropdownOption> CustomerDropdownOptions => Set<CustomerDropdownOption>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(user => user.DisplayName).HasMaxLength(120);
            entity.Property(user => user.CreatedAt).HasDefaultValueSql("timezone('utc', now())");
        });

        builder.Entity<Customer>(entity =>
        {
            entity.Property(customer => customer.Type)
                .HasConversion<string>()
                .HasMaxLength(20);
            entity.Property(customer => customer.Status)
                .HasConversion<string>()
                .HasMaxLength(20);
            entity.Property(customer => customer.DocumentNumber).HasMaxLength(14);
            entity.Property(customer => customer.Name).HasMaxLength(160);
            entity.Property(customer => customer.Email).HasMaxLength(256);
            entity.Property(customer => customer.BusinessPhone).HasMaxLength(30);
            entity.Property(customer => customer.MobilePhone).HasMaxLength(30);
            entity.Property(customer => customer.Website).HasMaxLength(200);
            entity.Property(customer => customer.CompanyName).HasMaxLength(160);
            entity.Property(customer => customer.Origin).HasMaxLength(80);
            entity.Property(customer => customer.Notes).HasMaxLength(1000);
            entity.Property(customer => customer.CreatedAt).HasDefaultValueSql("timezone('utc', now())");

            entity.HasIndex(customer => customer.DocumentNumber).IsUnique();

            entity.HasOne(customer => customer.Seller)
                .WithMany()
                .HasForeignKey(customer => customer.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<CustomerAddress>(entity =>
        {
            entity.Property(address => address.Type).HasMaxLength(80);
            entity.Property(address => address.PostalCode).HasMaxLength(8);
            entity.Property(address => address.Street).HasMaxLength(160);
            entity.Property(address => address.Number).HasMaxLength(20);
            entity.Property(address => address.Neighborhood).HasMaxLength(100);
            entity.Property(address => address.City).HasMaxLength(100);
            entity.Property(address => address.State).HasMaxLength(2);
            entity.Property(address => address.Complement).HasMaxLength(120);
            entity.Property(address => address.CreatedAt).HasDefaultValueSql("timezone('utc', now())");

            entity.HasIndex(address => address.CustomerId);

            entity.HasOne(address => address.Customer)
                .WithMany(customer => customer.Addresses)
                .HasForeignKey(address => address.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<CustomerDropdownOption>(entity =>
        {
            entity.Property(option => option.Kind)
                .HasConversion<string>()
                .HasMaxLength(30);
            entity.Property(option => option.Name).HasMaxLength(80);
            entity.Property(option => option.NormalizedName).HasMaxLength(80);
            entity.Property(option => option.CreatedAt).HasDefaultValueSql("timezone('utc', now())");

            entity.HasIndex(option => new { option.Kind, option.NormalizedName }).IsUnique();
        });
    }
}
