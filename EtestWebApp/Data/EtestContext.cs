using System;
using System.Collections.Generic;
using EtestWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EtestWebApp.Data;

public partial class EtestContext : DbContext
{
    public EtestContext()
    {
    }

    public EtestContext(DbContextOptions<EtestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kodg> Kodgs { get; set; }

    public virtual DbSet<Korisnik> Korisniks { get; set; }

    public virtual DbSet<Kt> Kts { get; set; }

    public virtual DbSet<Odgovor> Odgovors { get; set; }

    public virtual DbSet<Prasanje> Prasanjes { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; initial catalog = Etest; persist security info = True; integrated security = SSPI; trust server certificate = true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kodg>(entity =>
        {
            entity.ToTable("KOdg");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ktid).HasColumnName("KTID");
            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Pid).HasColumnName("PID");

            entity.HasOne(d => d.Kt).WithMany(p => p.Kodgs)
                .HasForeignKey(d => d.Ktid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KOdg2");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.Kodgs)
                .HasForeignKey(d => d.Oid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KOdg3");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Kodgs)
                .HasForeignKey(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KOdg1");
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.ToTable("Korisnik");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Kt>(entity =>
        {
            entity.ToTable("KT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Kid).HasColumnName("KID");
            entity.Property(e => e.Tid).HasColumnName("TID");

            entity.HasOne(d => d.KidNavigation).WithMany(p => p.Kts)
                .HasForeignKey(d => d.Kid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KT1");

            entity.HasOne(d => d.TidNavigation).WithMany(p => p.Kts)
                .HasForeignKey(d => d.Tid)
                .HasConstraintName("FK_KT2");
        });

        modelBuilder.Entity<Odgovor>(entity =>
        {
            entity.ToTable("Odgovor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Pid).HasColumnName("PID");
            entity.Property(e => e.Tekst).HasColumnType("text");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Odgovors)
                .HasForeignKey(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Odgovor");
        });

        modelBuilder.Entity<Prasanje>(entity =>
        {
            entity.ToTable("Prasanje");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Tekst).HasColumnType("text");
            entity.Property(e => e.Tid).HasColumnName("TID");

            entity.HasOne(d => d.TidNavigation).WithMany(p => p.Prasanjes)
                .HasForeignKey(d => d.Tid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prasanje");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.ToTable("Test");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ime)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Tip)
                .HasColumnName("Tip")
                .HasColumnType("bit") 
                .IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
