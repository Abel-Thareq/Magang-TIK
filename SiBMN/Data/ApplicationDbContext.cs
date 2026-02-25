using Microsoft.EntityFrameworkCore;
using SiBMN.Models;

namespace SiBMN.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<KategoriBarang> KategoriBarangs { get; set; }
        public DbSet<MasterBarang> MasterBarangs { get; set; }
        public DbSet<Pengajuan> Pengajuans { get; set; }
        public DbSet<DetailPengajuan> DetailPengajuans { get; set; }
        public DbSet<StokPersediaan> StokPersediaans { get; set; }
        public DbSet<AsetInventaris> AsetInventaris { get; set; }
        public DbSet<RuangGedung> RuangGedungs { get; set; }
        public DbSet<PenerimaanBarang> PenerimaanBarangs { get; set; }
        public DbSet<MutasiAset> MutasiAsets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Unit)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pengajuan>()
                .HasOne(p => p.Unit)
                .WithMany(u => u.Pengajuans)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pengajuan>()
                .HasOne(p => p.Pejabat)
                .WithMany()
                .HasForeignKey(p => p.IdPejabat)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetailPengajuan>()
                .HasOne(d => d.Pengajuan)
                .WithMany(p => p.DetailPengajuans)
                .HasForeignKey(d => d.IdPengajuan)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetailPengajuan>()
                .HasOne(d => d.MasterBarang)
                .WithMany(m => m.DetailPengajuans)
                .HasForeignKey(d => d.IdBarang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetailPengajuan>()
                .HasOne(d => d.RuangGedung)
                .WithMany(r => r.DetailPengajuans)
                .HasForeignKey(d => d.IdRuang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MasterBarang>()
                .HasOne(m => m.KategoriBarang)
                .WithMany(k => k.MasterBarangs)
                .HasForeignKey(m => m.IdKategori)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StokPersediaan>()
                .HasOne(s => s.MasterBarang)
                .WithMany(m => m.StokPersediaans)
                .HasForeignKey(s => s.IdBarang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StokPersediaan>()
                .HasOne(s => s.Unit)
                .WithMany(u => u.StokPersediaans)
                .HasForeignKey(s => s.IdUnit)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsetInventaris>()
                .HasOne(a => a.MasterBarang)
                .WithMany(m => m.AsetInventaris)
                .HasForeignKey(a => a.IdBarang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsetInventaris>()
                .HasOne(a => a.RuangGedung)
                .WithMany(r => r.AsetInventaris)
                .HasForeignKey(a => a.IdRuang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RuangGedung>()
                .HasOne(r => r.Unit)
                .WithMany(u => u.RuangGedungs)
                .HasForeignKey(r => r.IdUnit)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PenerimaanBarang>()
                .HasOne(p => p.Pengajuan)
                .WithMany(p => p.PenerimaanBarangs)
                .HasForeignKey(p => p.IdPengajuan)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MutasiAset>()
                .HasOne(m => m.AsetInventaris)
                .WithMany(a => a.MutasiAsets)
                .HasForeignKey(m => m.IdAset)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MutasiAset>()
                .HasOne(m => m.RuangGedung)
                .WithMany(r => r.MutasiAsets)
                .HasForeignKey(m => m.IdRuang)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { IdRole = 1, NamaRole = "Admin Unit Kerja" },
                new Role { IdRole = 2, NamaRole = "Tim Pengadaan" },
                new Role { IdRole = 3, NamaRole = "Admin Pusat" }
            );

            // Units
            modelBuilder.Entity<Unit>().HasData(
                new Unit { IdUnit = 1, NamaUnit = "Fakultas Teknik" },
                new Unit { IdUnit = 2, NamaUnit = "Fakultas Ekonomi" },
                new Unit { IdUnit = 3, NamaUnit = "Fakultas Hukum" },
                new Unit { IdUnit = 4, NamaUnit = "Rektorat" },
                new Unit { IdUnit = 5, NamaUnit = "UPT Perpustakaan" }
            );

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { IdUser = 1, Nama = "Admin Teknik", Email = "admin.teknik@univ.ac.id", Password = "admin123", RoleId = 1, UnitId = 1 },
                new User { IdUser = 2, Nama = "Admin Ekonomi", Email = "admin.ekonomi@univ.ac.id", Password = "admin123", RoleId = 1, UnitId = 2 },
                new User { IdUser = 3, Nama = "Tim Pengadaan", Email = "pengadaan@univ.ac.id", Password = "admin123", RoleId = 2, UnitId = 4 },
                new User { IdUser = 4, Nama = "Prof. Dr. Budi Santoso", Email = "budi@univ.ac.id", Password = "admin123", RoleId = 3, UnitId = 4 },
                new User { IdUser = 5, Nama = "Dr. Siti Rahayu, M.Sc.", Email = "siti@univ.ac.id", Password = "admin123", RoleId = 3, UnitId = 4 }
            );

            // Kategori Barang
            modelBuilder.Entity<KategoriBarang>().HasData(
                new KategoriBarang { IdKategori = 1, NamaKategori = "Peralatan dan Mesin" },
                new KategoriBarang { IdKategori = 2, NamaKategori = "Gedung dan Bangunan" },
                new KategoriBarang { IdKategori = 3, NamaKategori = "Jalan, Irigasi dan Jaringan" },
                new KategoriBarang { IdKategori = 4, NamaKategori = "Aset Tetap Lainnya" }
            );

            // Master Barang
            modelBuilder.Entity<MasterBarang>().HasData(
                new MasterBarang { IdBarang = 1, IdKategori = 1, NamaBarang = "Laptop", Spesifikasi = "Laptop untuk kebutuhan kantor", Satuan = "Unit" },
                new MasterBarang { IdBarang = 2, IdKategori = 1, NamaBarang = "Printer Laser", Spesifikasi = "Printer laser monochrome", Satuan = "Unit" },
                new MasterBarang { IdBarang = 3, IdKategori = 1, NamaBarang = "Proyektor", Spesifikasi = "Proyektor LCD", Satuan = "Unit" },
                new MasterBarang { IdBarang = 4, IdKategori = 1, NamaBarang = "AC Split", Spesifikasi = "AC Split 1 PK", Satuan = "Unit" },
                new MasterBarang { IdBarang = 5, IdKategori = 1, NamaBarang = "Komputer Desktop", Spesifikasi = "PC Desktop untuk kantor", Satuan = "Unit" },
                new MasterBarang { IdBarang = 6, IdKategori = 1, NamaBarang = "Scanner", Spesifikasi = "Scanner dokumen", Satuan = "Unit" },
                new MasterBarang { IdBarang = 7, IdKategori = 1, NamaBarang = "UPS", Spesifikasi = "Uninterruptible Power Supply", Satuan = "Unit" },
                new MasterBarang { IdBarang = 8, IdKategori = 1, NamaBarang = "Server Rack", Spesifikasi = "Server rack 42U", Satuan = "Unit" },
                new MasterBarang { IdBarang = 9, IdKategori = 1, NamaBarang = "Meja Kerja", Spesifikasi = "Meja kerja kayu", Satuan = "Buah" },
                new MasterBarang { IdBarang = 10, IdKategori = 1, NamaBarang = "Kursi Kerja", Spesifikasi = "Kursi kerja ergonomis", Satuan = "Buah" },
                new MasterBarang { IdBarang = 11, IdKategori = 1, NamaBarang = "Lemari Arsip", Spesifikasi = "Lemari arsip besi", Satuan = "Buah" },
                new MasterBarang { IdBarang = 12, IdKategori = 1, NamaBarang = "Whiteboard", Spesifikasi = "Papan tulis whiteboard", Satuan = "Buah" },
                new MasterBarang { IdBarang = 13, IdKategori = 1, NamaBarang = "Monitor LED", Spesifikasi = "Monitor LED 24 inch", Satuan = "Unit" },
                new MasterBarang { IdBarang = 14, IdKategori = 1, NamaBarang = "Router WiFi", Spesifikasi = "Router wireless enterprise", Satuan = "Unit" },
                new MasterBarang { IdBarang = 15, IdKategori = 1, NamaBarang = "CCTV Camera", Spesifikasi = "IP Camera outdoor", Satuan = "Unit" }
            );

            // Ruang Gedung
            modelBuilder.Entity<RuangGedung>().HasData(
                new RuangGedung { IdRuang = 1, NamaGedung = "Gedung A - Rektorat", NamaRuang = "Ruang Rapat Utama", IdUnit = 4 },
                new RuangGedung { IdRuang = 2, NamaGedung = "Gedung A - Rektorat", NamaRuang = "Ruang Kerja Lantai 1", IdUnit = 4 },
                new RuangGedung { IdRuang = 3, NamaGedung = "Gedung B - Fakultas Teknik", NamaRuang = "Lab Komputer 1", IdUnit = 1 },
                new RuangGedung { IdRuang = 4, NamaGedung = "Gedung B - Fakultas Teknik", NamaRuang = "Lab Komputer 2", IdUnit = 1 },
                new RuangGedung { IdRuang = 5, NamaGedung = "Gedung B - Fakultas Teknik", NamaRuang = "Ruang Dosen", IdUnit = 1 },
                new RuangGedung { IdRuang = 6, NamaGedung = "Gedung C - Fakultas Ekonomi", NamaRuang = "Ruang Kelas 101", IdUnit = 2 },
                new RuangGedung { IdRuang = 7, NamaGedung = "Gedung C - Fakultas Ekonomi", NamaRuang = "Ruang Dosen", IdUnit = 2 },
                new RuangGedung { IdRuang = 8, NamaGedung = "Gedung D - Fakultas Hukum", NamaRuang = "Ruang Sidang", IdUnit = 3 },
                new RuangGedung { IdRuang = 9, NamaGedung = "Gedung E - Perpustakaan", NamaRuang = "Ruang Baca", IdUnit = 5 },
                new RuangGedung { IdRuang = 10, NamaGedung = "Gedung E - Perpustakaan", NamaRuang = "Ruang Server", IdUnit = 5 }
            );
        }
    }
}
