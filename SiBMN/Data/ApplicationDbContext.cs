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
        public DbSet<KodeBarang> KodeBarangs { get; set; }

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
                .HasOne(d => d.KodeBarang)
                .WithMany()
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

            // KodeBarang unique index
            modelBuilder.Entity<KodeBarang>()
                .HasIndex(k => new { k.KodeGolongan, k.KodeBidang, k.KodeKelompok, k.KodeSubKelompok, k.KodeBarangValue })
                .IsUnique();

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { IdRole = 1, NamaRole = "Admin Unit Kerja" },
                new Role { IdRole = 2, NamaRole = "Tim Pengadaan" },
                new Role { IdRole = 3, NamaRole = "Admin Pusat" },
                new Role { IdRole = 4, NamaRole = "Tim Kerja BMN" }
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
                new User { IdUser = 5, Nama = "Dr. Siti Rahayu, M.Sc.", Email = "siti@univ.ac.id", Password = "admin123", RoleId = 3, UnitId = 4 },
                new User { IdUser = 6, Nama = "Tim BMN", Email = "bmn@univ.ac.id", Password = "admin123", RoleId = 4, UnitId = 4 }
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

            // Kode Barang (comprehensive seed data)
            modelBuilder.Entity<KodeBarang>().HasData(
                // === GOLONGAN ===
                new KodeBarang { Id = 1, KodeGolongan = "1", KodeBidang = "00", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Barang Kuasi" },
                new KodeBarang { Id = 2, KodeGolongan = "2", KodeBidang = "00", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Tanah" },
                new KodeBarang { Id = 3, KodeGolongan = "3", KodeBidang = "00", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Peralatan dan Mesin" },
                new KodeBarang { Id = 4, KodeGolongan = "4", KodeBidang = "00", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Gedung dan Bangunan" },
                new KodeBarang { Id = 5, KodeGolongan = "5", KodeBidang = "00", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Jalan, Irigasi dan Jaringan" },
                new KodeBarang { Id = 6, KodeGolongan = "6", KodeBidang = "00", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Aset Tetap Lainnya" },

                // === BIDANG di bawah Golongan 3 (Peralatan dan Mesin) ===
                new KodeBarang { Id = 10, KodeGolongan = "3", KodeBidang = "01", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Besar" },
                new KodeBarang { Id = 11, KodeGolongan = "3", KodeBidang = "02", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Angkutan" },
                new KodeBarang { Id = 12, KodeGolongan = "3", KodeBidang = "03", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Bengkel dan Alat Ukur" },
                new KodeBarang { Id = 13, KodeGolongan = "3", KodeBidang = "04", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Pertanian" },
                new KodeBarang { Id = 14, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Kantor dan Rumah Tangga" },
                new KodeBarang { Id = 15, KodeGolongan = "3", KodeBidang = "06", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Studio, Komunikasi dan Pemancar" },
                new KodeBarang { Id = 16, KodeGolongan = "3", KodeBidang = "07", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Kedokteran dan Kesehatan" },
                new KodeBarang { Id = 17, KodeGolongan = "3", KodeBidang = "08", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Laboratorium" },
                new KodeBarang { Id = 18, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Komputer" },

                // === BIDANG di bawah Golongan 4 (Gedung dan Bangunan) ===
                new KodeBarang { Id = 19, KodeGolongan = "4", KodeBidang = "01", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Bangunan Gedung" },
                new KodeBarang { Id = 20, KodeGolongan = "4", KodeBidang = "02", KodeKelompok = "00", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Monumen" },

                // === KELOMPOK di bawah 3.05 (Alat Kantor dan Rumah Tangga) ===
                new KodeBarang { Id = 21, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Kantor" },
                new KodeBarang { Id = 22, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Rumah Tangga" },
                new KodeBarang { Id = 23, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "03", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Peralatan Komputer" },

                // === KELOMPOK di bawah 3.09 (Komputer) ===
                new KodeBarang { Id = 24, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Komputer Unit" },
                new KodeBarang { Id = 25, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Peralatan Komputer" },
                new KodeBarang { Id = 26, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "03", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Peralatan Jaringan" },

                // === KELOMPOK di bawah 3.01 (Alat Besar) ===
                new KodeBarang { Id = 27, KodeGolongan = "3", KodeBidang = "01", KodeKelompok = "01", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Besar Darat" },
                new KodeBarang { Id = 28, KodeGolongan = "3", KodeBidang = "01", KodeKelompok = "02", KodeSubKelompok = "00", KodeBarangValue = "000", UraianBarang = "Alat Besar Apung" },

                // === SUB KELOMPOK di bawah 3.05.01 (Alat Kantor) ===
                new KodeBarang { Id = 30, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "000", UraianBarang = "Mesin Ketik" },
                new KodeBarang { Id = 31, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "02", KodeBarangValue = "000", UraianBarang = "Mesin Hitung/Jumlah" },
                new KodeBarang { Id = 32, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "000", UraianBarang = "Alat Penyimpanan Perlengkapan Kantor" },
                new KodeBarang { Id = 33, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "04", KodeBarangValue = "000", UraianBarang = "Alat Kantor Lainnya" },

                // === SUB KELOMPOK di bawah 3.05.02 (Alat Rumah Tangga) ===
                new KodeBarang { Id = 34, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "000", UraianBarang = "Meubelair" },
                new KodeBarang { Id = 35, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "02", KodeBarangValue = "000", UraianBarang = "Alat Pendingin" },
                new KodeBarang { Id = 36, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "000", UraianBarang = "Alat Dapur" },
                new KodeBarang { Id = 37, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "04", KodeBarangValue = "000", UraianBarang = "Alat Pembersih" },

                // === SUB KELOMPOK di bawah 3.09.01 (Komputer Unit) ===
                new KodeBarang { Id = 38, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "000", UraianBarang = "Komputer/PC" },
                new KodeBarang { Id = 39, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "02", KodeBarangValue = "000", UraianBarang = "Laptop/Notebook" },
                new KodeBarang { Id = 40, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "000", UraianBarang = "Server" },

                // === SUB KELOMPOK di bawah 3.09.02 (Peralatan Komputer) ===
                new KodeBarang { Id = 41, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "000", UraianBarang = "Peralatan Mainframe" },
                new KodeBarang { Id = 42, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "02", KodeBarangValue = "000", UraianBarang = "Peralatan Mini Komputer" },
                new KodeBarang { Id = 43, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "000", UraianBarang = "Peralatan Personal Komputer" },

                // === KODE BARANG di bawah 3.05.01.03 (Alat Penyimpanan) ===
                new KodeBarang { Id = 50, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "001", UraianBarang = "Lemari Besi/Metal" },
                new KodeBarang { Id = 51, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "002", UraianBarang = "Lemari Kayu" },
                new KodeBarang { Id = 52, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "003", UraianBarang = "Rak Besi/Metal" },
                new KodeBarang { Id = 53, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "004", UraianBarang = "Rak Kayu" },
                new KodeBarang { Id = 54, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "005", UraianBarang = "Filing Cabinet Besi" },
                new KodeBarang { Id = 55, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "006", UraianBarang = "Brand Kas" },

                // === KODE BARANG di bawah 3.05.02.01 (Meubelair) ===
                new KodeBarang { Id = 56, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "001", UraianBarang = "Meja Kerja" },
                new KodeBarang { Id = 57, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "002", UraianBarang = "Meja Rapat" },
                new KodeBarang { Id = 58, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "003", UraianBarang = "Kursi Kerja" },
                new KodeBarang { Id = 59, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "004", UraianBarang = "Kursi Tamu" },
                new KodeBarang { Id = 60, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "005", UraianBarang = "Tempat Tidur" },
                new KodeBarang { Id = 61, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "01", KodeBarangValue = "006", UraianBarang = "Sofa" },

                // === KODE BARANG di bawah 3.05.02.02 (Alat Pendingin) ===
                new KodeBarang { Id = 62, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "02", KodeBarangValue = "001", UraianBarang = "AC Split" },
                new KodeBarang { Id = 63, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "02", KodeBarangValue = "002", UraianBarang = "AC Window" },
                new KodeBarang { Id = 64, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "02", KodeBarangValue = "003", UraianBarang = "Kipas Angin" },
                new KodeBarang { Id = 65, KodeGolongan = "3", KodeBidang = "05", KodeKelompok = "02", KodeSubKelompok = "02", KodeBarangValue = "004", UraianBarang = "Exhaust Fan" },

                // === KODE BARANG di bawah 3.09.01.01 (Komputer/PC) ===
                new KodeBarang { Id = 66, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "001", UraianBarang = "PC Desktop" },
                new KodeBarang { Id = 67, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "002", UraianBarang = "PC All-in-One" },
                new KodeBarang { Id = 68, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "003", UraianBarang = "PC Mini/NUC" },

                // === KODE BARANG di bawah 3.09.01.02 (Laptop/Notebook) ===
                new KodeBarang { Id = 69, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "02", KodeBarangValue = "001", UraianBarang = "Laptop 14 inch" },
                new KodeBarang { Id = 70, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "02", KodeBarangValue = "002", UraianBarang = "Laptop 15 inch" },
                new KodeBarang { Id = 71, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "02", KodeBarangValue = "003", UraianBarang = "Notebook Ultrabook" },

                // === KODE BARANG di bawah 3.09.02.03 (Peralatan Personal Komputer) ===
                new KodeBarang { Id = 72, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "001", UraianBarang = "Printer Laser" },
                new KodeBarang { Id = 73, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "002", UraianBarang = "Printer Inkjet" },
                new KodeBarang { Id = 74, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "003", UraianBarang = "Scanner" },
                new KodeBarang { Id = 75, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "004", UraianBarang = "Monitor LED" },
                new KodeBarang { Id = 76, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "005", UraianBarang = "Proyektor LCD" },
                new KodeBarang { Id = 77, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "006", UraianBarang = "UPS" },
                new KodeBarang { Id = 78, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "007", UraianBarang = "Keyboard" },
                new KodeBarang { Id = 79, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "02", KodeSubKelompok = "03", KodeBarangValue = "008", UraianBarang = "Mouse" },

                // === KODE BARANG di bawah 3.09.03 (Peralatan Jaringan) - kelompok + sub + barang ===
                new KodeBarang { Id = 80, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "03", KodeSubKelompok = "01", KodeBarangValue = "000", UraianBarang = "Peralatan Jaringan LAN" },
                new KodeBarang { Id = 81, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "03", KodeSubKelompok = "01", KodeBarangValue = "001", UraianBarang = "Router WiFi" },
                new KodeBarang { Id = 82, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "03", KodeSubKelompok = "01", KodeBarangValue = "002", UraianBarang = "Switch Managed" },
                new KodeBarang { Id = 83, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "03", KodeSubKelompok = "01", KodeBarangValue = "003", UraianBarang = "Access Point" },
                new KodeBarang { Id = 84, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "03", KodeSubKelompok = "01", KodeBarangValue = "004", UraianBarang = "CCTV IP Camera" },

                // === KODE BARANG di bawah 3.09.01.03 (Server) ===
                new KodeBarang { Id = 85, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "001", UraianBarang = "Server Tower" },
                new KodeBarang { Id = 86, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "002", UraianBarang = "Server Rack" },
                new KodeBarang { Id = 87, KodeGolongan = "3", KodeBidang = "09", KodeKelompok = "01", KodeSubKelompok = "03", KodeBarangValue = "003", UraianBarang = "Server Blade" },

                // === KODE BARANG di bawah 3.01.01 (Alat Besar Darat) ===
                new KodeBarang { Id = 88, KodeGolongan = "3", KodeBidang = "01", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "000", UraianBarang = "Traktor" },
                new KodeBarang { Id = 89, KodeGolongan = "3", KodeBidang = "01", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "001", UraianBarang = "Traktor Roda Dua" },
                new KodeBarang { Id = 90, KodeGolongan = "3", KodeBidang = "01", KodeKelompok = "01", KodeSubKelompok = "01", KodeBarangValue = "002", UraianBarang = "Traktor Roda Empat" }
            );
        }
    }
}
