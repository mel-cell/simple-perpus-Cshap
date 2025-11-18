using System;
using Microsoft.EntityFrameworkCore;
using pr.Models;
using Npgsql;

namespace pr.Data;

public class PerpusDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Buku> Buku { get; set; }
    public DbSet<Penulis> Penulis { get; set; }
    public DbSet<Penerbit> Penerbit { get; set; }
    public DbSet<Rak> Rak { get; set; }
    public DbSet<Siswa> Siswa { get; set; }
    public DbSet<Kategori> Kategori { get; set; }
    public DbSet<Peminjaman> Peminjaman { get; set; }
    public DbSet<Pengembalian> Pengembalian { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connString = "Host=localhost;Database=perpus;Username=postgres;Password=root";

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
        dataSourceBuilder.MapEnum<UserRole>("user_level_enum");
        var dataSource = dataSourceBuilder.Build();

        optionsBuilder.UseNpgsql(dataSource);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Penulis>().ToTable("penulis");
        modelBuilder.Entity<Kategori>().ToTable("kategori");
        modelBuilder.Entity<Penerbit>().ToTable("penerbit");
        modelBuilder.Entity<Rak>().ToTable("rak");
        modelBuilder.Entity<Buku>().ToTable("buku");
        modelBuilder.Entity<Peminjaman>().ToTable("peminjaman");
        // Add other tables as needed, e.g., if peminjaman_detail exists
        // modelBuilder.Entity<PeminjamanDetail>().ToTable("peminjaman_detail");

        base.OnModelCreating(modelBuilder);
    }
}
