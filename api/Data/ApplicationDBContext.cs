using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "b0d124b9-593c-4f54-94ed-d90ec1e19100",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "fb4a7c95-b85c-4450-8922-5ec66c751678",
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<Stock>(entity =>
            {
                entity.Property(s => s.Symbol)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(s => s.CompanyName)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(s => s.Industry)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(s => s.Symbol)
                    .IsUnique();
            });

            builder.Entity<Comment>(entity =>
            {
                entity.Property(c => c.Title)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(c => c.Content)
                    .HasColumnType("text")
                    .IsRequired();
            });
        }
    }
}