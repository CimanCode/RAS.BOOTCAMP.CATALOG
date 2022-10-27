using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RAS.BOOTCAMP.TEMPLATE.Datas.Entities;

namespace RAS.BOOTCAMP.TEMPLATE.Datas
{
    public partial class KampDbContext : DbContext
    {
        public KampDbContext()
        {
        }

        public KampDbContext(DbContextOptions<KampDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Barang> Barangs { get; set; } = null!;
        public virtual DbSet<ItemTransaksi> ItemTransakses { get; set; } = null!;
        public virtual DbSet<Keranjang> Keranjangs { get; set; } = null!;
        public virtual DbSet<Pembely> Pembelies { get; set; } = null!;
        public virtual DbSet<Penjual> Penjuals { get; set; } = null!;
        public virtual DbSet<Transaksi> Transakses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost; Port=5432; Database=KampDb; User Id=postgres; Password=bismillah12;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Barang>(entity =>
            {
                entity.HasKey(e => e.IdBarang);

                entity.ToTable("Barang");

                entity.HasIndex(e => e.IdPenjual, "IX_Barang_IdPenjual");

                entity.Property(e => e.FileName).HasMaxLength(250);

                entity.Property(e => e.Nama).HasMaxLength(10);

                entity.Property(e => e.Stok).HasColumnName("stok");

                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .HasColumnName("URL");

                entity.HasOne(d => d.IdPenjualNavigation)
                    .WithMany(p => p.Barangs)
                    .HasForeignKey(d => d.IdPenjual);
            });

            modelBuilder.Entity<ItemTransaksi>(entity =>
            {
                entity.ToTable("ItemTransaksis");

                entity.HasIndex(e => e.IdBarang, "IX_ItemTransaksis_IdBarang");

                entity.HasIndex(e => e.IdTransaksi, "IX_ItemTransaksis_IdTransaksi");

                entity.HasOne(d => d.IdBarangNavigation)
                    .WithMany(p => p.ItemTransaksis)
                    .HasForeignKey(d => d.IdBarang);

                entity.HasOne(d => d.IdTransaksiNavigation)
                    .WithMany(p => p.ItemTransaksis)
                    .HasForeignKey(d => d.IdTransaksi);
            });

            modelBuilder.Entity<Keranjang>(entity =>
            {
                entity.HasIndex(e => e.IdBarang, "IX_Keranjangs_IdBarang");

                entity.HasIndex(e => e.IdUser, "IX_Keranjangs_IdUser");

                entity.HasOne(d => d.IdBarangNavigation)
                    .WithMany(p => p.Keranjangs)
                    .HasForeignKey(d => d.IdBarang);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Keranjangs)
                    .HasForeignKey(d => d.IdUser);
            });

            modelBuilder.Entity<Pembely>(entity =>
            {
                entity.HasKey(e => e.IdPembeli);

                entity.HasIndex(e => e.IdUser, "IX_Pembelies_IdUser");

                entity.Property(e => e.Alamat).HasMaxLength(12);

                entity.Property(e => e.Nama).HasMaxLength(20);

                entity.Property(e => e.NoHandPhone).HasColumnName("No_HandPhone");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Pembelies)
                    .HasForeignKey(d => d.IdUser);
            });

            modelBuilder.Entity<Penjual>(entity =>
            {
                entity.HasKey(e => e.IdPenjual);

                entity.ToTable("Penjual");

                entity.HasIndex(e => e.IdUser, "IX_Penjual_IdUser");

                entity.Property(e => e.Alamat).HasMaxLength(20);

                entity.Property(e => e.NamaToko)
                    .HasMaxLength(20)
                    .HasColumnName("Nama_Toko");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Penjuals)
                    .HasForeignKey(d => d.IdUser);
            });

            modelBuilder.Entity<Transaksi>(entity =>
            {
                entity.HasKey(e => e.IdTransaksi);

                entity.ToTable("Transaksis");

                entity.HasIndex(e => e.IdUser, "IX_Transaksis_IdUser");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Transaksis)
                    .HasForeignKey(d => d.IdUser);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Tipe)
                    .HasMaxLength(20)
                    .HasColumnName("tipe");

                entity.Property(e => e.Username).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
