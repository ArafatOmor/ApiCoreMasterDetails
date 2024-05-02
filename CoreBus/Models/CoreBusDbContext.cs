using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoreBus.Models;

public partial class CoreBusDbContext : DbContext
{
    public CoreBusDbContext()
    {
    }

    public CoreBusDbContext(DbContextOptions<CoreBusDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BusType> BusTypes { get; set; }

    public virtual DbSet<Passanger> Passangers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=CoreBusDB; Trusted_Connection=true; TrustServerCertificate=true; MultipleActiveResultSets=true; Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusType>(entity =>
        {
            entity.HasKey(e => e.BusTypeId).HasName("PK__BusType__84A10CE82A365280");

            entity.ToTable("BusType");

            entity.Property(e => e.SeatFear).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Passanger).WithMany(p => p.BusTypes)
                .HasForeignKey(d => d.PassangerId)
                .HasConstraintName("FK__BusType__Passang__286302EC");
        });

        modelBuilder.Entity<Passanger>(entity =>
        {
            entity.HasKey(e => e.PassangerId).HasName("PK__Passange__859B7BC603BBC976");

            entity.ToTable("Passanger");

            entity.Property(e => e.ImageName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.JournyDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CD815980D");

            entity.ToTable("User");

            entity.Property(e => e.EmailId).IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
