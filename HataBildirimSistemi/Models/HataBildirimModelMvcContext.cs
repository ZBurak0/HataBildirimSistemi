using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual DbSet<ArızaBildirim> ArızaBildirim { get; set; }
        public virtual DbSet<ArızaTur> ArızaTur { get; set; }
        public virtual DbSet<Birim> Birim { get; set; }
        public virtual DbSet<Durum> Durum { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }


        public virtual DbSet<AltArizaTur> AltArizaTur { get; set; }

        IEnumerable<HataBildirimSistemi.Models.AltArizaTur> AltArizaTurleri { get; set; }
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
