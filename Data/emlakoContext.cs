using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Emlakkko
{
    public partial class emlakoContext : DbContext
    {
        public emlakoContext()
        {
        }

        public emlakoContext(DbContextOptions<emlakoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Adres> Adres { get; set; }
        public virtual DbSet<Esya> Esya { get; set; }
        public virtual DbSet<Ev> Ev { get; set; }
        public virtual DbSet<EvKira> EvKira { get; set; }
        public virtual DbSet<EvSahibi> EvSahibi { get; set; }
        public virtual DbSet<Fotograf> Fotograf { get; set; }
        public virtual DbSet<IlanKoy> IlanKoy { get; set; }
        public virtual DbSet<Kiraci> Kiraci { get; set; }
        public virtual DbSet<Oda> Oda { get; set; }
        public virtual DbSet<Ofis> Ofis { get; set; }
        public virtual DbSet<Ozellik> Ozellik { get; set; }
        public virtual DbSet<Personel> Personel { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=emlako;Username=postgres;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.Id)
                    .HasConstraintName("useradmin");
            });

            modelBuilder.Entity<Adres>(entity =>
            {
                entity.ToTable("adres");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EvId).HasColumnName("ev_id");

                entity.Property(e => e.Il)
                    .IsRequired()
                    .HasColumnName("il")
                    .HasMaxLength(2044);

                entity.Property(e => e.Ilce)
                    .IsRequired()
                    .HasColumnName("ilce")
                    .HasMaxLength(2044);

                entity.Property(e => e.Satir1)
                    .IsRequired()
                    .HasColumnName("satir1")
                    .HasMaxLength(2044);
            });

            modelBuilder.Entity<Esya>(entity =>
            {
                entity.ToTable("esya");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EsyaTipi)
                    .IsRequired()
                    .HasColumnName("esya_tipi")
                    .HasMaxLength(2044);

                entity.Property(e => e.EvId).HasColumnName("ev_id");

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.Esya)
                    .HasForeignKey(d => d.EvId)
                    .HasConstraintName("ev_esya");
            });

            modelBuilder.Entity<Ev>(entity =>
            {
                entity.ToTable("ev");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdresId).HasColumnName("adres_id");

                entity.Property(e => e.EvSahibiId).HasColumnName("ev_sahibi_id");

                entity.Property(e => e.EvTipi)
                    .IsRequired()
                    .HasColumnName("ev_tipi")
                    .HasMaxLength(2044);

                entity.Property(e => e.KiraFiyati).HasColumnName("kira_fiyati");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(2044);

                entity.HasOne(d => d.Adres)
                    .WithMany(p => p.Ev)
                    .HasForeignKey(d => d.AdresId)
                    .HasConstraintName("ev_adress");

                entity.HasOne(d => d.EvSahibi)
                    .WithMany(p => p.Ev)
                    .HasForeignKey(d => d.EvSahibiId)
                    .HasConstraintName("ev_sahibi");
            });

            modelBuilder.Entity<EvKira>(entity =>
            {
                entity.ToTable("ev_kira");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EvId).HasColumnName("ev_id");

                entity.Property(e => e.KiraFiyati).HasColumnName("kira_fiyati");

                entity.Property(e => e.KiraciId).HasColumnName("kiraci_id");

                entity.Property(e => e.PersonelId).HasColumnName("personel_id");

                entity.Property(e => e.Sure)
                    .IsRequired()
                    .HasColumnName("sure")
                    .HasMaxLength(2044);

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.EvKira)
                    .HasForeignKey(d => d.EvId)
                    .HasConstraintName("ev_kira_ev");

                entity.HasOne(d => d.Kiraci)
                    .WithMany(p => p.EvKira)
                    .HasForeignKey(d => d.KiraciId)
                    .HasConstraintName("kiraci-evkira");

                entity.HasOne(d => d.Personel)
                    .WithMany(p => p.EvKira)
                    .HasForeignKey(d => d.PersonelId)
                    .HasConstraintName("ev_kira_personel");
            });

            modelBuilder.Entity<EvSahibi>(entity =>
            {
                entity.ToTable("ev_sahibi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasColumnName("ad")
                    .HasMaxLength(2044);

                entity.Property(e => e.Soyad)
                    .IsRequired()
                    .HasColumnName("soyad")
                    .HasMaxLength(2044);

                entity.Property(e => e.Telefon)
                    .IsRequired()
                    .HasColumnName("telefon")
                    .HasMaxLength(2044);
            });

            modelBuilder.Entity<Fotograf>(entity =>
            {
                entity.ToTable("fotograf");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EvId).HasColumnName("ev_id");

                entity.Property(e => e.File)
                    .IsRequired()
                    .HasColumnName("file")
                    .HasMaxLength(2044);

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.Fotograf)
                    .HasForeignKey(d => d.EvId)
                    .HasConstraintName("ev_fotograf");
            });

            modelBuilder.Entity<IlanKoy>(entity =>
            {
                entity.ToTable("ilan_koy");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EvId).HasColumnName("ev_id");

                entity.Property(e => e.OfisId).HasColumnName("ofis_id");

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.IlanKoy)
                    .HasForeignKey(d => d.EvId)
                    .HasConstraintName("ev_ilan");

                entity.HasOne(d => d.Ofis)
                    .WithMany(p => p.IlanKoy)
                    .HasForeignKey(d => d.OfisId)
                    .HasConstraintName("ofis_ilan");
            });

            modelBuilder.Entity<Kiraci>(entity =>
            {
                entity.ToTable("kiraci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasColumnName("ad")
                    .HasMaxLength(2044);

                entity.Property(e => e.Soyad)
                    .IsRequired()
                    .HasColumnName("soyad")
                    .HasMaxLength(2044);
            });

            modelBuilder.Entity<Oda>(entity =>
            {
                entity.ToTable("oda");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EvId).HasColumnName("ev_id");

                entity.Property(e => e.OdaTipi)
                    .IsRequired()
                    .HasColumnName("oda_tipi")
                    .HasMaxLength(2044);

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.Oda)
                    .HasForeignKey(d => d.EvId)
                    .HasConstraintName("ev_oda");
            });

            modelBuilder.Entity<Ofis>(entity =>
            {
                entity.ToTable("ofis");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(2044);
            });

            modelBuilder.Entity<Ozellik>(entity =>
            {
                entity.ToTable("ozellik");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EvId).HasColumnName("ev_id");

                entity.Property(e => e.OzellikTipi)
                    .IsRequired()
                    .HasColumnName("ozellik_tipi")
                    .HasMaxLength(2044);

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.Ozellik)
                    .HasForeignKey(d => d.EvId)
                    .HasConstraintName("ev_ozellik");
            });

            modelBuilder.Entity<Personel>(entity =>
            {
                entity.ToTable("personel");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.OfisId).HasColumnName("ofis_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Personel)
                    .HasForeignKey<Personel>(d => d.Id)
                    .HasConstraintName("personeluser");

                entity.HasOne(d => d.Ofis)
                    .WithMany(p => p.Personel)
                    .HasForeignKey(d => d.OfisId)
                    .HasConstraintName("personel_ofis");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Email)
                    .HasName("unique_user_email")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasColumnName("ad")
                    .HasMaxLength(2044);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(2044);

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasColumnName("hash")
                    .HasMaxLength(2044);

                entity.Property(e => e.KisiTuru)
                    .IsRequired()
                    .HasColumnName("kisi_turu")
                    .HasMaxLength(2044);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasMaxLength(2044);

                entity.Property(e => e.Soyad)
                    .IsRequired()
                    .HasColumnName("soyad")
                    .HasMaxLength(2044);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
