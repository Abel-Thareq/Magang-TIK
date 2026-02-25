using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SiBMN.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategori_Barang",
                columns: table => new
                {
                    id_kategori = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nama_kategori = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategori_Barang", x => x.id_kategori);
                });

            migrationBuilder.CreateTable(
                name: "Kode_Barang",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    kode_golongan = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    kode_bidang = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    kode_kelompok = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    kode_sub_kelompok = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    kode_barang_value = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    uraian_barang = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kode_Barang", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id_role = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nama_role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id_role);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    id_unit = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nama_unit = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.id_unit);
                });

            migrationBuilder.CreateTable(
                name: "Master_Barang",
                columns: table => new
                {
                    id_barang = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_kategori = table.Column<int>(type: "INTEGER", nullable: false),
                    nama_barang = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    spesifikasi = table.Column<string>(type: "TEXT", nullable: true),
                    satuan = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_Barang", x => x.id_barang);
                    table.ForeignKey(
                        name: "FK_Master_Barang_Kategori_Barang_id_kategori",
                        column: x => x.id_kategori,
                        principalTable: "Kategori_Barang",
                        principalColumn: "id_kategori",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ruang_Gedung",
                columns: table => new
                {
                    id_ruang = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nama_gedung = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    nama_ruang = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    id_unit = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruang_Gedung", x => x.id_ruang);
                    table.ForeignKey(
                        name: "FK_Ruang_Gedung_Units_id_unit",
                        column: x => x.id_unit,
                        principalTable: "Units",
                        principalColumn: "id_unit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nama = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    role_id = table.Column<int>(type: "INTEGER", nullable: false),
                    unit_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id_user);
                    table.ForeignKey(
                        name: "FK_Users_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id_role",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "Units",
                        principalColumn: "id_unit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stok_Persediaan",
                columns: table => new
                {
                    id_stok = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_barang = table.Column<int>(type: "INTEGER", nullable: false),
                    id_unit = table.Column<int>(type: "INTEGER", nullable: false),
                    jumlah_stok = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stok_Persediaan", x => x.id_stok);
                    table.ForeignKey(
                        name: "FK_Stok_Persediaan_Master_Barang_id_barang",
                        column: x => x.id_barang,
                        principalTable: "Master_Barang",
                        principalColumn: "id_barang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stok_Persediaan_Units_id_unit",
                        column: x => x.id_unit,
                        principalTable: "Units",
                        principalColumn: "id_unit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Aset_Inventaris",
                columns: table => new
                {
                    id_aset = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_barang = table.Column<int>(type: "INTEGER", nullable: false),
                    kode_inventaris = table.Column<int>(type: "INTEGER", nullable: false),
                    kondisi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    id_ruang = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aset_Inventaris", x => x.id_aset);
                    table.ForeignKey(
                        name: "FK_Aset_Inventaris_Master_Barang_id_barang",
                        column: x => x.id_barang,
                        principalTable: "Master_Barang",
                        principalColumn: "id_barang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aset_Inventaris_Ruang_Gedung_id_ruang",
                        column: x => x.id_ruang,
                        principalTable: "Ruang_Gedung",
                        principalColumn: "id_ruang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pengajuan",
                columns: table => new
                {
                    id_pengajuan = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nomor_surat = table.Column<int>(type: "INTEGER", nullable: true),
                    no_surat_rektor = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    tgl_surat_rektor = table.Column<DateTime>(type: "TEXT", nullable: true),
                    id_pejabat = table.Column<int>(type: "INTEGER", nullable: true),
                    jabatan = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    total_harga = table.Column<decimal>(type: "TEXT", nullable: false),
                    unit_id = table.Column<int>(type: "INTEGER", nullable: false),
                    tanggal_pengajuan = table.Column<DateTime>(type: "TEXT", nullable: false),
                    jenis_pengajuan = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    tahun_anggaran = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pengajuan", x => x.id_pengajuan);
                    table.ForeignKey(
                        name: "FK_Pengajuan_Units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "Units",
                        principalColumn: "id_unit",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pengajuan_Users_id_pejabat",
                        column: x => x.id_pejabat,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mutasi_Aset",
                columns: table => new
                {
                    id_mutasi = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_aset = table.Column<int>(type: "INTEGER", nullable: false),
                    id_ruang = table.Column<int>(type: "INTEGER", nullable: false),
                    tanggal_mutasi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mutasi_Aset", x => x.id_mutasi);
                    table.ForeignKey(
                        name: "FK_Mutasi_Aset_Aset_Inventaris_id_aset",
                        column: x => x.id_aset,
                        principalTable: "Aset_Inventaris",
                        principalColumn: "id_aset",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mutasi_Aset_Ruang_Gedung_id_ruang",
                        column: x => x.id_ruang,
                        principalTable: "Ruang_Gedung",
                        principalColumn: "id_ruang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Detail_Pengajuan",
                columns: table => new
                {
                    id_detPengajuan = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_pengajuan = table.Column<int>(type: "INTEGER", nullable: false),
                    id_barang = table.Column<int>(type: "INTEGER", nullable: false),
                    no_prioritas = table.Column<int>(type: "INTEGER", nullable: false),
                    id_ruang = table.Column<int>(type: "INTEGER", nullable: false),
                    jumlah_diminta = table.Column<int>(type: "INTEGER", nullable: false),
                    harga_satuan = table.Column<decimal>(type: "TEXT", nullable: false),
                    jumlah_harga = table.Column<decimal>(type: "TEXT", nullable: false),
                    fungsi_barang = table.Column<string>(type: "TEXT", nullable: true),
                    asal_barang = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    alasan_import = table.Column<string>(type: "TEXT", nullable: true),
                    link_gambar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    jumlah_disetujui = table.Column<int>(type: "INTEGER", nullable: false),
                    link_survey = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    gambar_ekatalog = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    spesifikasi = table.Column<string>(type: "TEXT", nullable: true),
                    MasterBarangIdBarang = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail_Pengajuan", x => x.id_detPengajuan);
                    table.ForeignKey(
                        name: "FK_Detail_Pengajuan_Kode_Barang_id_barang",
                        column: x => x.id_barang,
                        principalTable: "Kode_Barang",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Detail_Pengajuan_Master_Barang_MasterBarangIdBarang",
                        column: x => x.MasterBarangIdBarang,
                        principalTable: "Master_Barang",
                        principalColumn: "id_barang");
                    table.ForeignKey(
                        name: "FK_Detail_Pengajuan_Pengajuan_id_pengajuan",
                        column: x => x.id_pengajuan,
                        principalTable: "Pengajuan",
                        principalColumn: "id_pengajuan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detail_Pengajuan_Ruang_Gedung_id_ruang",
                        column: x => x.id_ruang,
                        principalTable: "Ruang_Gedung",
                        principalColumn: "id_ruang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Penerimaan_Barang",
                columns: table => new
                {
                    id_penerimaan = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_pengajuan = table.Column<int>(type: "INTEGER", nullable: false),
                    tanggal_terima = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penerimaan_Barang", x => x.id_penerimaan);
                    table.ForeignKey(
                        name: "FK_Penerimaan_Barang_Pengajuan_id_pengajuan",
                        column: x => x.id_pengajuan,
                        principalTable: "Pengajuan",
                        principalColumn: "id_pengajuan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kategori_Barang",
                columns: new[] { "id_kategori", "nama_kategori" },
                values: new object[,]
                {
                    { 1, "Peralatan dan Mesin" },
                    { 2, "Gedung dan Bangunan" },
                    { 3, "Jalan, Irigasi dan Jaringan" },
                    { 4, "Aset Tetap Lainnya" }
                });

            migrationBuilder.InsertData(
                table: "Kode_Barang",
                columns: new[] { "id", "kode_barang_value", "kode_bidang", "kode_golongan", "kode_kelompok", "kode_sub_kelompok", "uraian_barang" },
                values: new object[,]
                {
                    { 1, "000", "00", "1", "00", "00", "Barang Kuasi" },
                    { 2, "000", "00", "2", "00", "00", "Tanah" },
                    { 3, "000", "00", "3", "00", "00", "Peralatan dan Mesin" },
                    { 4, "000", "00", "4", "00", "00", "Gedung dan Bangunan" },
                    { 5, "000", "00", "5", "00", "00", "Jalan, Irigasi dan Jaringan" },
                    { 6, "000", "00", "6", "00", "00", "Aset Tetap Lainnya" },
                    { 10, "000", "01", "3", "00", "00", "Alat Besar" },
                    { 11, "000", "02", "3", "00", "00", "Alat Angkutan" },
                    { 12, "000", "03", "3", "00", "00", "Alat Bengkel dan Alat Ukur" },
                    { 13, "000", "04", "3", "00", "00", "Alat Pertanian" },
                    { 14, "000", "05", "3", "00", "00", "Alat Kantor dan Rumah Tangga" },
                    { 15, "000", "06", "3", "00", "00", "Alat Studio, Komunikasi dan Pemancar" },
                    { 16, "000", "07", "3", "00", "00", "Alat Kedokteran dan Kesehatan" },
                    { 17, "000", "08", "3", "00", "00", "Alat Laboratorium" },
                    { 18, "000", "09", "3", "00", "00", "Komputer" },
                    { 19, "000", "01", "4", "00", "00", "Bangunan Gedung" },
                    { 20, "000", "02", "4", "00", "00", "Monumen" },
                    { 21, "000", "05", "3", "01", "00", "Alat Kantor" },
                    { 22, "000", "05", "3", "02", "00", "Alat Rumah Tangga" },
                    { 23, "000", "05", "3", "03", "00", "Peralatan Komputer" },
                    { 24, "000", "09", "3", "01", "00", "Komputer Unit" },
                    { 25, "000", "09", "3", "02", "00", "Peralatan Komputer" },
                    { 26, "000", "09", "3", "03", "00", "Peralatan Jaringan" },
                    { 27, "000", "01", "3", "01", "00", "Alat Besar Darat" },
                    { 28, "000", "01", "3", "02", "00", "Alat Besar Apung" },
                    { 30, "000", "05", "3", "01", "01", "Mesin Ketik" },
                    { 31, "000", "05", "3", "01", "02", "Mesin Hitung/Jumlah" },
                    { 32, "000", "05", "3", "01", "03", "Alat Penyimpanan Perlengkapan Kantor" },
                    { 33, "000", "05", "3", "01", "04", "Alat Kantor Lainnya" },
                    { 34, "000", "05", "3", "02", "01", "Meubelair" },
                    { 35, "000", "05", "3", "02", "02", "Alat Pendingin" },
                    { 36, "000", "05", "3", "02", "03", "Alat Dapur" },
                    { 37, "000", "05", "3", "02", "04", "Alat Pembersih" },
                    { 38, "000", "09", "3", "01", "01", "Komputer/PC" },
                    { 39, "000", "09", "3", "01", "02", "Laptop/Notebook" },
                    { 40, "000", "09", "3", "01", "03", "Server" },
                    { 41, "000", "09", "3", "02", "01", "Peralatan Mainframe" },
                    { 42, "000", "09", "3", "02", "02", "Peralatan Mini Komputer" },
                    { 43, "000", "09", "3", "02", "03", "Peralatan Personal Komputer" },
                    { 50, "001", "05", "3", "01", "03", "Lemari Besi/Metal" },
                    { 51, "002", "05", "3", "01", "03", "Lemari Kayu" },
                    { 52, "003", "05", "3", "01", "03", "Rak Besi/Metal" },
                    { 53, "004", "05", "3", "01", "03", "Rak Kayu" },
                    { 54, "005", "05", "3", "01", "03", "Filing Cabinet Besi" },
                    { 55, "006", "05", "3", "01", "03", "Brand Kas" },
                    { 56, "001", "05", "3", "02", "01", "Meja Kerja" },
                    { 57, "002", "05", "3", "02", "01", "Meja Rapat" },
                    { 58, "003", "05", "3", "02", "01", "Kursi Kerja" },
                    { 59, "004", "05", "3", "02", "01", "Kursi Tamu" },
                    { 60, "005", "05", "3", "02", "01", "Tempat Tidur" },
                    { 61, "006", "05", "3", "02", "01", "Sofa" },
                    { 62, "001", "05", "3", "02", "02", "AC Split" },
                    { 63, "002", "05", "3", "02", "02", "AC Window" },
                    { 64, "003", "05", "3", "02", "02", "Kipas Angin" },
                    { 65, "004", "05", "3", "02", "02", "Exhaust Fan" },
                    { 66, "001", "09", "3", "01", "01", "PC Desktop" },
                    { 67, "002", "09", "3", "01", "01", "PC All-in-One" },
                    { 68, "003", "09", "3", "01", "01", "PC Mini/NUC" },
                    { 69, "001", "09", "3", "01", "02", "Laptop 14 inch" },
                    { 70, "002", "09", "3", "01", "02", "Laptop 15 inch" },
                    { 71, "003", "09", "3", "01", "02", "Notebook Ultrabook" },
                    { 72, "001", "09", "3", "02", "03", "Printer Laser" },
                    { 73, "002", "09", "3", "02", "03", "Printer Inkjet" },
                    { 74, "003", "09", "3", "02", "03", "Scanner" },
                    { 75, "004", "09", "3", "02", "03", "Monitor LED" },
                    { 76, "005", "09", "3", "02", "03", "Proyektor LCD" },
                    { 77, "006", "09", "3", "02", "03", "UPS" },
                    { 78, "007", "09", "3", "02", "03", "Keyboard" },
                    { 79, "008", "09", "3", "02", "03", "Mouse" },
                    { 80, "000", "09", "3", "03", "01", "Peralatan Jaringan LAN" },
                    { 81, "001", "09", "3", "03", "01", "Router WiFi" },
                    { 82, "002", "09", "3", "03", "01", "Switch Managed" },
                    { 83, "003", "09", "3", "03", "01", "Access Point" },
                    { 84, "004", "09", "3", "03", "01", "CCTV IP Camera" },
                    { 85, "001", "09", "3", "01", "03", "Server Tower" },
                    { 86, "002", "09", "3", "01", "03", "Server Rack" },
                    { 87, "003", "09", "3", "01", "03", "Server Blade" },
                    { 88, "000", "01", "3", "01", "01", "Traktor" },
                    { 89, "001", "01", "3", "01", "01", "Traktor Roda Dua" },
                    { 90, "002", "01", "3", "01", "01", "Traktor Roda Empat" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id_role", "nama_role" },
                values: new object[,]
                {
                    { 1, "Admin Unit Kerja" },
                    { 2, "Tim Pengadaan" },
                    { 3, "Admin Pusat" },
                    { 4, "Tim Kerja BMN" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "id_unit", "nama_unit" },
                values: new object[,]
                {
                    { 1, "Fakultas Teknik" },
                    { 2, "Fakultas Ekonomi" },
                    { 3, "Fakultas Hukum" },
                    { 4, "Rektorat" },
                    { 5, "UPT Perpustakaan" }
                });

            migrationBuilder.InsertData(
                table: "Master_Barang",
                columns: new[] { "id_barang", "id_kategori", "nama_barang", "satuan", "spesifikasi" },
                values: new object[,]
                {
                    { 1, 1, "Laptop", "Unit", "Laptop untuk kebutuhan kantor" },
                    { 2, 1, "Printer Laser", "Unit", "Printer laser monochrome" },
                    { 3, 1, "Proyektor", "Unit", "Proyektor LCD" },
                    { 4, 1, "AC Split", "Unit", "AC Split 1 PK" },
                    { 5, 1, "Komputer Desktop", "Unit", "PC Desktop untuk kantor" },
                    { 6, 1, "Scanner", "Unit", "Scanner dokumen" },
                    { 7, 1, "UPS", "Unit", "Uninterruptible Power Supply" },
                    { 8, 1, "Server Rack", "Unit", "Server rack 42U" },
                    { 9, 1, "Meja Kerja", "Buah", "Meja kerja kayu" },
                    { 10, 1, "Kursi Kerja", "Buah", "Kursi kerja ergonomis" },
                    { 11, 1, "Lemari Arsip", "Buah", "Lemari arsip besi" },
                    { 12, 1, "Whiteboard", "Buah", "Papan tulis whiteboard" },
                    { 13, 1, "Monitor LED", "Unit", "Monitor LED 24 inch" },
                    { 14, 1, "Router WiFi", "Unit", "Router wireless enterprise" },
                    { 15, 1, "CCTV Camera", "Unit", "IP Camera outdoor" }
                });

            migrationBuilder.InsertData(
                table: "Ruang_Gedung",
                columns: new[] { "id_ruang", "id_unit", "nama_gedung", "nama_ruang" },
                values: new object[,]
                {
                    { 1, 4, "Gedung A - Rektorat", "Ruang Rapat Utama" },
                    { 2, 4, "Gedung A - Rektorat", "Ruang Kerja Lantai 1" },
                    { 3, 1, "Gedung B - Fakultas Teknik", "Lab Komputer 1" },
                    { 4, 1, "Gedung B - Fakultas Teknik", "Lab Komputer 2" },
                    { 5, 1, "Gedung B - Fakultas Teknik", "Ruang Dosen" },
                    { 6, 2, "Gedung C - Fakultas Ekonomi", "Ruang Kelas 101" },
                    { 7, 2, "Gedung C - Fakultas Ekonomi", "Ruang Dosen" },
                    { 8, 3, "Gedung D - Fakultas Hukum", "Ruang Sidang" },
                    { 9, 5, "Gedung E - Perpustakaan", "Ruang Baca" },
                    { 10, 5, "Gedung E - Perpustakaan", "Ruang Server" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id_user", "email", "nama", "password", "role_id", "unit_id" },
                values: new object[,]
                {
                    { 1, "admin.teknik@univ.ac.id", "Admin Teknik", "admin123", 1, 1 },
                    { 2, "admin.ekonomi@univ.ac.id", "Admin Ekonomi", "admin123", 1, 2 },
                    { 3, "pengadaan@univ.ac.id", "Tim Pengadaan", "admin123", 2, 4 },
                    { 4, "budi@univ.ac.id", "Prof. Dr. Budi Santoso", "admin123", 3, 4 },
                    { 5, "siti@univ.ac.id", "Dr. Siti Rahayu, M.Sc.", "admin123", 3, 4 },
                    { 6, "bmn@univ.ac.id", "Tim BMN", "admin123", 4, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aset_Inventaris_id_barang",
                table: "Aset_Inventaris",
                column: "id_barang");

            migrationBuilder.CreateIndex(
                name: "IX_Aset_Inventaris_id_ruang",
                table: "Aset_Inventaris",
                column: "id_ruang");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_Pengajuan_id_barang",
                table: "Detail_Pengajuan",
                column: "id_barang");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_Pengajuan_id_pengajuan",
                table: "Detail_Pengajuan",
                column: "id_pengajuan");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_Pengajuan_id_ruang",
                table: "Detail_Pengajuan",
                column: "id_ruang");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_Pengajuan_MasterBarangIdBarang",
                table: "Detail_Pengajuan",
                column: "MasterBarangIdBarang");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_Barang_kode_golongan_kode_bidang_kode_kelompok_kode_sub_kelompok_kode_barang_value",
                table: "Kode_Barang",
                columns: new[] { "kode_golongan", "kode_bidang", "kode_kelompok", "kode_sub_kelompok", "kode_barang_value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Master_Barang_id_kategori",
                table: "Master_Barang",
                column: "id_kategori");

            migrationBuilder.CreateIndex(
                name: "IX_Mutasi_Aset_id_aset",
                table: "Mutasi_Aset",
                column: "id_aset");

            migrationBuilder.CreateIndex(
                name: "IX_Mutasi_Aset_id_ruang",
                table: "Mutasi_Aset",
                column: "id_ruang");

            migrationBuilder.CreateIndex(
                name: "IX_Penerimaan_Barang_id_pengajuan",
                table: "Penerimaan_Barang",
                column: "id_pengajuan");

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_id_pejabat",
                table: "Pengajuan",
                column: "id_pejabat");

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_unit_id",
                table: "Pengajuan",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ruang_Gedung_id_unit",
                table: "Ruang_Gedung",
                column: "id_unit");

            migrationBuilder.CreateIndex(
                name: "IX_Stok_Persediaan_id_barang",
                table: "Stok_Persediaan",
                column: "id_barang");

            migrationBuilder.CreateIndex(
                name: "IX_Stok_Persediaan_id_unit",
                table: "Stok_Persediaan",
                column: "id_unit");

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_unit_id",
                table: "Users",
                column: "unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detail_Pengajuan");

            migrationBuilder.DropTable(
                name: "Mutasi_Aset");

            migrationBuilder.DropTable(
                name: "Penerimaan_Barang");

            migrationBuilder.DropTable(
                name: "Stok_Persediaan");

            migrationBuilder.DropTable(
                name: "Kode_Barang");

            migrationBuilder.DropTable(
                name: "Aset_Inventaris");

            migrationBuilder.DropTable(
                name: "Pengajuan");

            migrationBuilder.DropTable(
                name: "Master_Barang");

            migrationBuilder.DropTable(
                name: "Ruang_Gedung");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Kategori_Barang");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
