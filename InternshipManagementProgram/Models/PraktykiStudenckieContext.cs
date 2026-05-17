using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InternshipManagementProgram.Models;

public partial class PraktykiStudenckieContext : DbContext
{
    public PraktykiStudenckieContext(DbContextOptions<PraktykiStudenckieContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Firma> Firmas { get; set; }

    public virtual DbSet<Kierunek> Kieruneks { get; set; }

    public virtual DbSet<Opiekun> Opiekuns { get; set; }

    public virtual DbSet<OsobaKontaktowa> OsobaKontaktowas { get; set; }

    public virtual DbSet<Praktyka> Praktykas { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<VPraktykiPelne> VPraktykiPelnes { get; set; }

    public virtual DbSet<VPraktykiStatystyki> VPraktykiStatystykis { get; set; }

    public virtual DbSet<VStatystykiKierunkow> VStatystykiKierunkows { get; set; }

    public virtual DbSet<VStudenciBezPraktyk> VStudenciBezPraktyks { get; set; }

    public virtual DbSet<VStudenciNiezaliczeni> VStudenciNiezaliczenis { get; set; }

    public virtual DbSet<Wydzial> Wydzials { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Firma>(entity =>
        {
            entity.HasKey(e => e.Idfirmy);

            entity.ToTable("Firma");

            entity.Property(e => e.Idfirmy).HasColumnName("IDFirmy");
            entity.Property(e => e.Adres).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.KodPocztowy).HasMaxLength(10);
            entity.Property(e => e.Miasto).HasMaxLength(50);
            entity.Property(e => e.NazwaFirmy).HasMaxLength(100);
            entity.Property(e => e.Telefon).HasMaxLength(20);
        });

        modelBuilder.Entity<Kierunek>(entity =>
        {
            entity.HasKey(e => e.Idkierunku);

            entity.ToTable("Kierunek");

            entity.Property(e => e.Idkierunku).HasColumnName("IDKierunku");
            entity.Property(e => e.Idwydzialu).HasColumnName("IDWydzialu");
            entity.Property(e => e.NazwaKierunku).HasMaxLength(100);

            entity.HasOne(d => d.IdwydzialuNavigation).WithMany(p => p.Kieruneks)
                .HasForeignKey(d => d.Idwydzialu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kierunek_Wydzial");
        });

        modelBuilder.Entity<Opiekun>(entity =>
        {
            entity.HasKey(e => e.Idopiekuna);

            entity.ToTable("Opiekun");

            entity.HasIndex(e => e.Email, "UQ_Opiekun_Email").IsUnique();

            entity.Property(e => e.Idopiekuna).HasColumnName("IDOpiekuna");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(20);
            entity.Property(e => e.Tytul).HasMaxLength(50);
        });

        modelBuilder.Entity<OsobaKontaktowa>(entity =>
        {
            entity.HasKey(e => e.Idosoby);

            entity.ToTable("OsobaKontaktowa");

            entity.Property(e => e.Idosoby).HasColumnName("IDOsoby");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Idfirmy).HasColumnName("IDFirmy");
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
            entity.Property(e => e.Stanowisko).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(20);

            entity.HasOne(d => d.IdfirmyNavigation).WithMany(p => p.OsobaKontaktowas)
                .HasForeignKey(d => d.Idfirmy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OsobaKontaktowa_Firma");
        });

        modelBuilder.Entity<Praktyka>(entity =>
        {
            entity.HasKey(e => e.Idpraktyki);

            entity.ToTable("Praktyka", tb =>
                {
                    tb.HasTrigger("TR_Praktyka_DataPrzyZatwierdzeniu");
                    tb.HasTrigger("TR_Praktyka_KontrolaStatusu");
                    tb.HasTrigger("TR_Praktyka_NakladajaceSieDaty");
                    tb.HasTrigger("TR_Praktyka_OcenaStatus");
                    tb.HasTrigger("TR_Praktyka_WeryfikacjaOsobyKontaktowej");
                });

            entity.Property(e => e.Idpraktyki).HasColumnName("IDPraktyki");
            entity.Property(e => e.DataZgloszenia).HasDefaultValueSql("(getdate())", "DF_Praktyka_DataZgloszenia");
            entity.Property(e => e.Idfirmy).HasColumnName("IDFirmy");
            entity.Property(e => e.Idopiekuna).HasColumnName("IDOpiekuna");
            entity.Property(e => e.Idosoby).HasColumnName("IDOsoby");
            entity.Property(e => e.NrAlbumu).HasMaxLength(20);
            entity.Property(e => e.Ocena).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.RodzajPraktyki).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Zgłoszona", "DF_Praktyka_Status");

            entity.HasOne(d => d.IdfirmyNavigation).WithMany(p => p.Praktykas)
                .HasForeignKey(d => d.Idfirmy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Praktyka_Firma");

            entity.HasOne(d => d.IdopiekunaNavigation).WithMany(p => p.Praktykas)
                .HasForeignKey(d => d.Idopiekuna)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Praktyka_Opiekun");

            entity.HasOne(d => d.IdosobyNavigation).WithMany(p => p.Praktykas)
                .HasForeignKey(d => d.Idosoby)
                .HasConstraintName("FK_Praktyka_OsobaKontaktowa");

            entity.HasOne(d => d.NrAlbumuNavigation).WithMany(p => p.Praktykas)
                .HasForeignKey(d => d.NrAlbumu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Praktyka_Student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.NrAlbumu);

            entity.ToTable("Student");

            entity.HasIndex(e => e.Email, "UQ_Student_Email").IsUnique();

            entity.Property(e => e.NrAlbumu).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Idkierunku).HasColumnName("IDKierunku");
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(20);

            entity.HasOne(d => d.IdkierunkuNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Idkierunku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Kierunek");
        });

        modelBuilder.Entity<VPraktykiPelne>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_PraktykiPelne");

            entity.Property(e => e.AdresFirmy).HasMaxLength(200);
            entity.Property(e => e.EmailStudenta).HasMaxLength(100);
            entity.Property(e => e.Idfirmy).HasColumnName("IDFirmy");
            entity.Property(e => e.Idopiekuna).HasColumnName("IDOpiekuna");
            entity.Property(e => e.Idosoby).HasColumnName("IDOsoby");
            entity.Property(e => e.Idpraktyki).HasColumnName("IDPraktyki");
            entity.Property(e => e.ImieOpiekuna).HasMaxLength(50);
            entity.Property(e => e.ImieOsobyKontaktowej).HasMaxLength(50);
            entity.Property(e => e.ImieStudenta).HasMaxLength(50);
            entity.Property(e => e.MiastoFirmy).HasMaxLength(50);
            entity.Property(e => e.NazwaFirmy).HasMaxLength(100);
            entity.Property(e => e.NazwaKierunku).HasMaxLength(100);
            entity.Property(e => e.NazwaWydzialu).HasMaxLength(100);
            entity.Property(e => e.NazwiskoOpiekuna).HasMaxLength(50);
            entity.Property(e => e.NazwiskoOsobyKontaktowej).HasMaxLength(50);
            entity.Property(e => e.NazwiskoStudenta).HasMaxLength(50);
            entity.Property(e => e.NrAlbumu).HasMaxLength(20);
            entity.Property(e => e.Ocena).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.RodzajPraktyki).HasMaxLength(50);
            entity.Property(e => e.StanowiskoOsobyKontaktowej).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TytulOpiekuna).HasMaxLength(50);
        });

        modelBuilder.Entity<VPraktykiStatystyki>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_PraktykiStatystyki");

            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<VStatystykiKierunkow>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_StatystykiKierunkow");

            entity.Property(e => e.NazwaKierunku).HasMaxLength(100);
            entity.Property(e => e.ProcentZdawalnosci).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<VStudenciBezPraktyk>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_StudenciBezPraktyk");

            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.NazwaKierunku).HasMaxLength(100);
            entity.Property(e => e.NazwaWydzialu).HasMaxLength(100);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
            entity.Property(e => e.NrAlbumu).HasMaxLength(20);
        });

        modelBuilder.Entity<VStudenciNiezaliczeni>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_StudenciNiezaliczeni");

            entity.Property(e => e.Idpraktyki).HasColumnName("IDPraktyki");
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.NazwaFirmy).HasMaxLength(100);
            entity.Property(e => e.NazwaKierunku).HasMaxLength(100);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
            entity.Property(e => e.NrAlbumu).HasMaxLength(20);
            entity.Property(e => e.Ocena).HasColumnType("decimal(2, 1)");
        });

        modelBuilder.Entity<Wydzial>(entity =>
        {
            entity.HasKey(e => e.Idwydzialu);

            entity.ToTable("Wydzial");

            entity.HasIndex(e => e.NazwaWydzialu, "UQ_Wydzial_Nazwa").IsUnique();

            entity.Property(e => e.Idwydzialu).HasColumnName("IDWydzialu");
            entity.Property(e => e.NazwaWydzialu).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
