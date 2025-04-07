using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public partial class CoreArtDbContext : DbContext
{
    public CoreArtDbContext()
    {
    }

    public CoreArtDbContext(DbContextOptions<CoreArtDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Art> Arts { get; set; }

    public virtual DbSet<Exhibition> Exhibitions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CoreArtDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Art>(entity =>
        {
            entity.HasKey(e => e.ArtId).HasName("PK__Art__3FB70AE60A772A3B");

            entity.ToTable("Art");

            entity.Property(e => e.ArtName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ImageName).IsUnicode(false);
            entity.Property(e => e.ImageUrl).IsUnicode(false);
        });

        modelBuilder.Entity<Exhibition>(entity =>
        {
            entity.HasKey(e => e.ExhibitionId).HasName("PK__Exhibiti__32CDCC1E4DF394B8");

            entity.ToTable("Exhibition");

            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Art).WithMany(p => p.Exhibitions)
                .HasForeignKey(d => d.ArtId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Exhibitio__ArtId__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
