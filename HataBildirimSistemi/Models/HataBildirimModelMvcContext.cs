using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HataBildirimSistemi.Models
{
    public partial class HataBildirimModelMvcContext : DbContext
    {
        public HataBildirimModelMvcContext()
        {
        }

        public HataBildirimModelMvcContext(DbContextOptions<HataBildirimModelMvcContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<ArızaBildirim> ArızaBildirim { get; set; }
        public virtual DbSet<ArızaTur> ArızaTur { get; set; }
        public virtual DbSet<Birim> Birim { get; set; }
        public virtual DbSet<Durum> Durum { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }
        public virtual DbSet<YetkiliServis> YetkiliServis { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=ZIYABURAKYAYLA\\SQLEXPRESS;initial catalog=HataBildirimModelMvc;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.AKullaniciAd)
                    .HasColumnName("AKullaniciAd")
                    .HasMaxLength(30);

                entity.Property(e => e.ASifre)
                    .HasColumnName("ASifre")
                    .HasMaxLength(500);

                entity.Property(e => e.Soyad).HasMaxLength(50);

                entity.Property(e => e.TelNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Birim)
                    .WithMany(p => p.Admin)
                    .HasForeignKey(d => d.BirimId)
                    .HasConstraintName("FK_Admin_Birim");

                entity.HasOne(d => d.Yetki)
                    .WithMany(p => p.Admin)
                    .HasForeignKey(d => d.YetkiId)
                    .HasConstraintName("FK_Admin_Yetki");
            });

            modelBuilder.Entity<ArızaBildirim>(entity =>
            {
                entity.Property(e => e.Aciklama).HasMaxLength(350);

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.DosyaYolu).HasMaxLength(500);

                entity.Property(e => e.KullaniciAd).HasMaxLength(50);

                entity.Property(e => e.Tarih).HasColumnType("datetime");

                entity.HasOne(d => d.ArızaTur)
                    .WithMany(p => p.ArızaBildirim)
                    .HasForeignKey(d => d.ArizaTurId)
                    .HasConstraintName("FK_ArızaBildirim_ArızaTur");

                entity.HasOne(d => d.Birim)
                    .WithMany(p => p.ArızaBildirim)
                    .HasForeignKey(d => d.BirimId)
                    .HasConstraintName("FK_ArızaBildirim_Birim");
            });

            modelBuilder.Entity<ArızaTur>(entity =>
            {
                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<Birim>(entity =>
            {
                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<Durum>(entity =>
            {
                entity.Property(e => e.Ad)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Kullanici>(entity =>
            {
                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.KKullaniciAd)
                    .HasColumnName("KKullaniciAd")
                    .HasMaxLength(30);

                entity.Property(e => e.KSifre)
                    .HasColumnName("KSifre")
                    .HasMaxLength(500);

                entity.Property(e => e.Soyad).HasMaxLength(50);

                entity.Property(e => e.TelNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Birim)
                    .WithMany(p => p.Kullanici)
                    .HasForeignKey(d => d.BirimId)
                    .HasConstraintName("FK_Kullanici_Birim");

              

                entity.HasOne(d => d.Yetki)
                    .WithMany(p => p.Kullanici)
                    .HasForeignKey(d => d.YetkiId)
                    .HasConstraintName("FK_Kullanici_Yetki");
            });

            modelBuilder.Entity<Yetki>(entity =>
            {
                entity.Property(e => e.Ad).HasMaxLength(50);
            });

            modelBuilder.Entity<YetkiliServis>(entity =>
            {
                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Soyad).HasMaxLength(50);

                entity.Property(e => e.TelNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.YKullaniciAd)
                    .HasColumnName("YKullaniciAd")
                    .HasMaxLength(30);

                entity.Property(e => e.YSifre)
                    .HasColumnName("YSifre")
                    .HasMaxLength(500);

                entity.HasOne(d => d.ArızaTur)
                    .WithMany(p => p.YetkiliServis)
                    .HasForeignKey(d => d.ArizaTurId)
                    .HasConstraintName("FK_YetkiliServis_ArızaTur");

                entity.HasOne(d => d.Yetki)
                    .WithMany(p => p.YetkiliServis)
                    .HasForeignKey(d => d.YetkiId)
                    .HasConstraintName("FK_YetkiliServis_Yetki");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
