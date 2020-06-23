using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChemiClean.Models
{
    public partial class ChemiCleanDBContext : DbContext
    {
        public ChemiCleanDBContext()
        {
        }

        public ChemiCleanDBContext(DbContextOptions<ChemiCleanDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataSheets> DataSheets { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<TblProduct> TblProduct { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=ChemiCleanDB;Trusted_Connection=True; User Id=sd40;password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSheets>(entity =>
            {
                entity.HasKey(e => e.DataSheetId);

                entity.Property(e => e.DataSheetId).HasColumnName("DataSheetID");

                entity.Property(e => e.DataSheetUrl)
                    .IsRequired()
                    .HasColumnName("DataSheetURL")
                    .HasMaxLength(300);

                entity.Property(e => e.HashValue)
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.LocalUrl)
                    .HasColumnName("LocalURL")
                    .HasMaxLength(250);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(250);
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(e => e.SupplierId);

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.SupplierName).HasMaxLength(250);
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblProduct");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.SupplierName).HasMaxLength(250);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
