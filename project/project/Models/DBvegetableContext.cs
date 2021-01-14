using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace project.Models
{
    // DBvegetableContext class
    public partial class DBvegetableContext : DbContext
    {
        public DBvegetableContext()
        {
        }

        public DBvegetableContext(DbContextOptions<DBvegetableContext> options)
            : base(options)
        {
        }

        //Database tables
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vegetable> Vegetables { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DBvegetable");
            }
        }

        //Tables settings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("User");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Vegetable>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Vegetabl__737584F7B1BD2437");

                entity.ToTable("Vegetable");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(7);

                entity.Property(e => e.Size).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
