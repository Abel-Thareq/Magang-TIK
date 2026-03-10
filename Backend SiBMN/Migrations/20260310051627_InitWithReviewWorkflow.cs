using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SiBMN.Migrations
{
    /// <inheritdoc />
    public partial class InitWithReviewWorkflow : Migration
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
                    nama_kategori = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    uraian_barang = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    nama_role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    nama_unit = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    satuan = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    id_unit = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    unit_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    jumlah_stok = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    id_ruang = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                name: "JadwalEvents",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    bulan = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    waktu = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    keterangan = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JadwalEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_JadwalEvents_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
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
                    tahun_anggaran = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    reviewed_by = table.Column<int>(type: "INTEGER", nullable: true),
                    approved_by = table.Column<int>(type: "INTEGER", nullable: true)
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
                        name: "FK_Pengajuan_Users_approved_by",
                        column: x => x.approved_by,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pengajuan_Users_id_pejabat",
                        column: x => x.id_pejabat,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pengajuan_Users_reviewed_by",
                        column: x => x.reviewed_by,
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
                    tanggal_mutasi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    tanggal_terima = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                columns: new[] { "id_kategori", "created_at", "deleted_at", "nama_kategori", "updated_at" },
                values: new object[,]
                {
                    { 1, null, null, "Peralatan dan Mesin", null },
                    { 2, null, null, "Gedung dan Bangunan", null },
                    { 3, null, null, "Jalan, Irigasi dan Jaringan", null },
                    { 4, null, null, "Aset Tetap Lainnya", null }
                });

            migrationBuilder.InsertData(
                table: "Kode_Barang",
                columns: new[] { "id", "created_at", "deleted_at", "kode_barang_value", "kode_bidang", "kode_golongan", "kode_kelompok", "kode_sub_kelompok", "updated_at", "uraian_barang" },
                values: new object[,]
                {
                    { 1, null, null, "000", "00", "1", "00", "00", null, "Barang Kuasi" },
                    { 2, null, null, "000", "00", "2", "00", "00", null, "Tanah" },
                    { 3, null, null, "000", "00", "3", "00", "00", null, "Peralatan dan Mesin" },
                    { 4, null, null, "000", "00", "4", "00", "00", null, "Gedung dan Bangunan" },
                    { 5, null, null, "000", "00", "5", "00", "00", null, "Jalan, Irigasi dan Jaringan" },
                    { 6, null, null, "000", "00", "6", "00", "00", null, "Aset Tetap Lainnya" },
                    { 10, null, null, "000", "01", "3", "00", "00", null, "Alat Besar" },
                    { 11, null, null, "000", "02", "3", "00", "00", null, "Alat Angkutan" },
                    { 12, null, null, "000", "03", "3", "00", "00", null, "Alat Bengkel dan Alat Ukur" },
                    { 13, null, null, "000", "04", "3", "00", "00", null, "Alat Pertanian" },
                    { 14, null, null, "000", "05", "3", "00", "00", null, "Alat Kantor dan Rumah Tangga" },
                    { 15, null, null, "000", "06", "3", "00", "00", null, "Alat Studio, Komunikasi dan Pemancar" },
                    { 16, null, null, "000", "07", "3", "00", "00", null, "Alat Kedokteran dan Kesehatan" },
                    { 17, null, null, "000", "08", "3", "00", "00", null, "Alat Laboratorium" },
                    { 18, null, null, "000", "09", "3", "00", "00", null, "Komputer" },
                    { 19, null, null, "000", "01", "4", "00", "00", null, "Bangunan Gedung" },
                    { 20, null, null, "000", "02", "4", "00", "00", null, "Monumen" },
                    { 21, null, null, "000", "05", "3", "01", "00", null, "Alat Kantor" },
                    { 22, null, null, "000", "05", "3", "02", "00", null, "Alat Rumah Tangga" },
                    { 23, null, null, "000", "05", "3", "03", "00", null, "Peralatan Komputer" },
                    { 24, null, null, "000", "09", "3", "01", "00", null, "Komputer Unit" },
                    { 25, null, null, "000", "09", "3", "02", "00", null, "Peralatan Komputer" },
                    { 26, null, null, "000", "09", "3", "03", "00", null, "Peralatan Jaringan" },
                    { 27, null, null, "000", "01", "3", "01", "00", null, "Alat Besar Darat" },
                    { 28, null, null, "000", "01", "3", "02", "00", null, "Alat Besar Apung" },
                    { 30, null, null, "000", "05", "3", "01", "01", null, "Mesin Ketik" },
                    { 31, null, null, "000", "05", "3", "01", "02", null, "Mesin Hitung/Jumlah" },
                    { 32, null, null, "000", "05", "3", "01", "03", null, "Alat Penyimpanan Perlengkapan Kantor" },
                    { 33, null, null, "000", "05", "3", "01", "04", null, "Alat Kantor Lainnya" },
                    { 34, null, null, "000", "05", "3", "02", "01", null, "Meubelair" },
                    { 35, null, null, "000", "05", "3", "02", "02", null, "Alat Pendingin" },
                    { 36, null, null, "000", "05", "3", "02", "03", null, "Alat Dapur" },
                    { 37, null, null, "000", "05", "3", "02", "04", null, "Alat Pembersih" },
                    { 38, null, null, "000", "09", "3", "01", "01", null, "Komputer/PC" },
                    { 39, null, null, "000", "09", "3", "01", "02", null, "Laptop/Notebook" },
                    { 40, null, null, "000", "09", "3", "01", "03", null, "Server" },
                    { 41, null, null, "000", "09", "3", "02", "01", null, "Peralatan Mainframe" },
                    { 42, null, null, "000", "09", "3", "02", "02", null, "Peralatan Mini Komputer" },
                    { 43, null, null, "000", "09", "3", "02", "03", null, "Peralatan Personal Komputer" },
                    { 50, null, null, "001", "05", "3", "01", "03", null, "Lemari Besi/Metal" },
                    { 51, null, null, "002", "05", "3", "01", "03", null, "Lemari Kayu" },
                    { 52, null, null, "003", "05", "3", "01", "03", null, "Rak Besi/Metal" },
                    { 53, null, null, "004", "05", "3", "01", "03", null, "Rak Kayu" },
                    { 54, null, null, "005", "05", "3", "01", "03", null, "Filing Cabinet Besi" },
                    { 55, null, null, "006", "05", "3", "01", "03", null, "Brand Kas" },
                    { 56, null, null, "001", "05", "3", "02", "01", null, "Meja Kerja" },
                    { 57, null, null, "002", "05", "3", "02", "01", null, "Meja Rapat" },
                    { 58, null, null, "003", "05", "3", "02", "01", null, "Kursi Kerja" },
                    { 59, null, null, "004", "05", "3", "02", "01", null, "Kursi Tamu" },
                    { 60, null, null, "005", "05", "3", "02", "01", null, "Tempat Tidur" },
                    { 61, null, null, "006", "05", "3", "02", "01", null, "Sofa" },
                    { 62, null, null, "001", "05", "3", "02", "02", null, "AC Split" },
                    { 63, null, null, "002", "05", "3", "02", "02", null, "AC Window" },
                    { 64, null, null, "003", "05", "3", "02", "02", null, "Kipas Angin" },
                    { 65, null, null, "004", "05", "3", "02", "02", null, "Exhaust Fan" },
                    { 66, null, null, "001", "09", "3", "01", "01", null, "PC Desktop" },
                    { 67, null, null, "002", "09", "3", "01", "01", null, "PC All-in-One" },
                    { 68, null, null, "003", "09", "3", "01", "01", null, "PC Mini/NUC" },
                    { 69, null, null, "001", "09", "3", "01", "02", null, "Laptop 14 inch" },
                    { 70, null, null, "002", "09", "3", "01", "02", null, "Laptop 15 inch" },
                    { 71, null, null, "003", "09", "3", "01", "02", null, "Notebook Ultrabook" },
                    { 72, null, null, "001", "09", "3", "02", "03", null, "Printer Laser" },
                    { 73, null, null, "002", "09", "3", "02", "03", null, "Printer Inkjet" },
                    { 74, null, null, "003", "09", "3", "02", "03", null, "Scanner" },
                    { 75, null, null, "004", "09", "3", "02", "03", null, "Monitor LED" },
                    { 76, null, null, "005", "09", "3", "02", "03", null, "Proyektor LCD" },
                    { 77, null, null, "006", "09", "3", "02", "03", null, "UPS" },
                    { 78, null, null, "007", "09", "3", "02", "03", null, "Keyboard" },
                    { 79, null, null, "008", "09", "3", "02", "03", null, "Mouse" },
                    { 80, null, null, "000", "09", "3", "03", "01", null, "Peralatan Jaringan LAN" },
                    { 81, null, null, "001", "09", "3", "03", "01", null, "Router WiFi" },
                    { 82, null, null, "002", "09", "3", "03", "01", null, "Switch Managed" },
                    { 83, null, null, "003", "09", "3", "03", "01", null, "Access Point" },
                    { 84, null, null, "004", "09", "3", "03", "01", null, "CCTV IP Camera" },
                    { 85, null, null, "001", "09", "3", "01", "03", null, "Server Tower" },
                    { 86, null, null, "002", "09", "3", "01", "03", null, "Server Rack" },
                    { 87, null, null, "003", "09", "3", "01", "03", null, "Server Blade" },
                    { 88, null, null, "000", "01", "3", "01", "01", null, "Traktor" },
                    { 89, null, null, "001", "01", "3", "01", "01", null, "Traktor Roda Dua" },
                    { 90, null, null, "002", "01", "3", "01", "01", null, "Traktor Roda Empat" },
                    { 100, null, null, "000", "01", "4", "01", "00", null, "Bangunan Gedung Tempat Kerja" },
                    { 101, null, null, "000", "01", "4", "01", "01", null, "Gedung Kantor" },
                    { 102, null, null, "001", "01", "4", "01", "01", null, "Gedung Kantor Permanen" },
                    { 103, null, null, "002", "01", "4", "01", "01", null, "Gedung Kantor Semi Permanen" },
                    { 104, null, null, "000", "01", "4", "02", "00", null, "Bangunan Gedung Tempat Pendidikan" },
                    { 105, null, null, "000", "01", "4", "02", "01", null, "Gedung Perkuliahan" },
                    { 106, null, null, "001", "01", "4", "02", "01", null, "Gedung Kuliah A" },
                    { 107, null, null, "002", "01", "4", "02", "01", null, "Gedung Kuliah B" },
                    { 108, null, null, "003", "01", "4", "02", "01", null, "Gedung Laboratorium" },
                    { 109, null, null, "004", "01", "4", "02", "01", null, "Gedung Perpustakaan" },
                    { 110, null, null, "005", "01", "4", "02", "01", null, "Gedung Aula" },
                    { 120, null, null, "000", "01", "5", "00", "00", null, "Jalan dan Jembatan" },
                    { 121, null, null, "000", "01", "5", "01", "00", null, "Jalan" },
                    { 122, null, null, "000", "01", "5", "01", "01", null, "Jalan Kampus" },
                    { 123, null, null, "001", "01", "5", "01", "01", null, "Jalan Utama Kampus" },
                    { 124, null, null, "002", "01", "5", "01", "01", null, "Jalan Lingkungan Kampus" },
                    { 125, null, null, "000", "02", "5", "00", "00", null, "Bangunan Air/Irigasi" },
                    { 126, null, null, "000", "02", "5", "01", "00", null, "Bangunan Air Irigasi" },
                    { 127, null, null, "000", "02", "5", "01", "01", null, "Saluran Irigasi" },
                    { 128, null, null, "001", "02", "5", "01", "01", null, "Saluran Drainase Kampus" },
                    { 129, null, null, "000", "03", "5", "00", "00", null, "Instalasi" },
                    { 130, null, null, "000", "03", "5", "01", "00", null, "Instalasi Air Bersih" },
                    { 131, null, null, "000", "03", "5", "01", "01", null, "Instalasi Air Minum" },
                    { 132, null, null, "001", "03", "5", "01", "01", null, "Instalasi Air Bersih Kampus" },
                    { 133, null, null, "000", "04", "5", "00", "00", null, "Jaringan" },
                    { 134, null, null, "000", "04", "5", "01", "00", null, "Jaringan Listrik" },
                    { 135, null, null, "000", "04", "5", "01", "01", null, "Jaringan Listrik Kampus" },
                    { 136, null, null, "001", "04", "5", "01", "01", null, "Jaringan Listrik Gedung A" },
                    { 137, null, null, "002", "04", "5", "01", "01", null, "Jaringan Listrik Gedung B" },
                    { 140, null, null, "000", "01", "6", "00", "00", null, "Buku dan Perpustakaan" },
                    { 141, null, null, "000", "01", "6", "01", "00", null, "Buku" },
                    { 142, null, null, "000", "01", "6", "01", "01", null, "Buku Umum" },
                    { 143, null, null, "001", "01", "6", "01", "01", null, "Buku Referensi Teknik" },
                    { 144, null, null, "002", "01", "6", "01", "01", null, "Buku Referensi Ekonomi" },
                    { 145, null, null, "003", "01", "6", "01", "01", null, "Buku Referensi Hukum" },
                    { 146, null, null, "004", "01", "6", "01", "01", null, "Jurnal Ilmiah" },
                    { 147, null, null, "000", "02", "6", "00", "00", null, "Barang Bercorak Kesenian" },
                    { 148, null, null, "000", "02", "6", "01", "00", null, "Barang Bercorak Kebudayaan" },
                    { 149, null, null, "000", "02", "6", "01", "01", null, "Lukisan" },
                    { 150, null, null, "001", "02", "6", "01", "01", null, "Lukisan Dinding" },
                    { 151, null, null, "002", "02", "6", "01", "01", null, "Patung Kampus" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id_role", "created_at", "deleted_at", "nama_role", "updated_at" },
                values: new object[,]
                {
                    { 1, null, null, "Admin Unit Kerja", null },
                    { 2, null, null, "Tim Pengadaan", null },
                    { 3, null, null, "Admin Pusat", null },
                    { 4, null, null, "Tim Kerja BMN", null },
                    { 5, null, null, "Pimpinan BMN", null }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "id_unit", "created_at", "deleted_at", "nama_unit", "updated_at" },
                values: new object[,]
                {
                    { 1, null, null, "UPA TIK", null },
                    { 2, null, null, "UPA Bahasa", null },
                    { 3, null, null, "UPA Perpustakaan", null },
                    { 4, null, null, "Rektorat", null },
                    { 5, null, null, "UPA Karier & Kewirausahaan", null },
                    { 6, null, null, "UPA Taman Agroteknologi", null }
                });

            migrationBuilder.InsertData(
                table: "Master_Barang",
                columns: new[] { "id_barang", "created_at", "deleted_at", "id_kategori", "nama_barang", "satuan", "spesifikasi", "updated_at" },
                values: new object[,]
                {
                    { 1, null, null, 1, "Laptop", "Unit", "Laptop untuk kebutuhan kantor", null },
                    { 2, null, null, 1, "Printer Laser", "Unit", "Printer laser monochrome", null },
                    { 3, null, null, 1, "Proyektor", "Unit", "Proyektor LCD", null },
                    { 4, null, null, 1, "AC Split", "Unit", "AC Split 1 PK", null },
                    { 5, null, null, 1, "Komputer Desktop", "Unit", "PC Desktop untuk kantor", null },
                    { 6, null, null, 1, "Scanner", "Unit", "Scanner dokumen", null },
                    { 7, null, null, 1, "UPS", "Unit", "Uninterruptible Power Supply", null },
                    { 8, null, null, 1, "Server Rack", "Unit", "Server rack 42U", null },
                    { 9, null, null, 1, "Meja Kerja", "Buah", "Meja kerja kayu", null },
                    { 10, null, null, 1, "Kursi Kerja", "Buah", "Kursi kerja ergonomis", null },
                    { 11, null, null, 1, "Lemari Arsip", "Buah", "Lemari arsip besi", null },
                    { 12, null, null, 1, "Whiteboard", "Buah", "Papan tulis whiteboard", null },
                    { 13, null, null, 1, "Monitor LED", "Unit", "Monitor LED 24 inch", null },
                    { 14, null, null, 1, "Router WiFi", "Unit", "Router wireless enterprise", null },
                    { 15, null, null, 1, "CCTV Camera", "Unit", "IP Camera outdoor", null }
                });

            migrationBuilder.InsertData(
                table: "Pengajuan",
                columns: new[] { "id_pengajuan", "approved_by", "created_at", "deleted_at", "id_pejabat", "jabatan", "jenis_pengajuan", "no_surat_rektor", "nomor_surat", "reviewed_by", "status", "tahun_anggaran", "tanggal_pengajuan", "tgl_surat_rektor", "total_harga", "unit_id", "updated_at" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45000000m, 1, null },
                    { 2, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 32000000m, 1, null },
                    { 3, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 28000000m, 2, null },
                    { 4, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15500000m, 2, null },
                    { 5, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 52000000m, 3, null },
                    { 6, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 19000000m, 5, null },
                    { 7, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8500000m, 5, null },
                    { 8, null, null, null, null, null, "Belanja Modal", null, null, null, "draft", 2025, new DateTime(2025, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 37500000m, 6, null }
                });

            migrationBuilder.InsertData(
                table: "Ruang_Gedung",
                columns: new[] { "id_ruang", "created_at", "deleted_at", "id_unit", "nama_gedung", "nama_ruang", "updated_at" },
                values: new object[,]
                {
                    { 1, null, null, 4, "Gedung A - Rektorat", "Ruang Rapat Utama", null },
                    { 2, null, null, 4, "Gedung A - Rektorat", "Ruang Kerja Lantai 1", null },
                    { 3, null, null, 1, "Gedung B - Fakultas Teknik", "Lab Komputer 1", null },
                    { 4, null, null, 1, "Gedung B - Fakultas Teknik", "Lab Komputer 2", null },
                    { 5, null, null, 1, "Gedung B - Fakultas Teknik", "Ruang Dosen", null },
                    { 6, null, null, 2, "Gedung C - Fakultas Ekonomi", "Ruang Kelas 101", null },
                    { 7, null, null, 2, "Gedung C - Fakultas Ekonomi", "Ruang Dosen", null },
                    { 8, null, null, 3, "Gedung D - Fakultas Hukum", "Ruang Sidang", null },
                    { 9, null, null, 5, "Gedung E - Perpustakaan", "Ruang Baca", null },
                    { 10, null, null, 5, "Gedung E - Perpustakaan", "Ruang Server", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id_user", "created_at", "deleted_at", "email", "nama", "password", "role_id", "unit_id", "updated_at" },
                values: new object[,]
                {
                    { 1, null, null, "admin.upatik@univ.ac.id", "Admin UPA TIK", "admin123", 1, 1, null },
                    { 2, null, null, "admin.upabahasa@univ.ac.id", "Admin UPA Bahasa", "admin123", 1, 2, null },
                    { 3, null, null, "admin.upaperpustakaan@univ.ac.id", "Admin UPA Perpustakaan", "admin123", 1, 3, null },
                    { 4, null, null, "admin.upakarier@univ.ac.id", "Admin UPA Karier", "admin123", 1, 5, null },
                    { 5, null, null, "admin.upaagrotek@univ.ac.id", "Admin UPA Agroteknologi", "admin123", 1, 6, null },
                    { 6, null, null, "pengadaan@univ.ac.id", "Tim Pengadaan", "admin123", 2, 4, null },
                    { 7, null, null, "budi@univ.ac.id", "Prof. Dr. Budi Santoso", "admin123", 3, 4, null },
                    { 8, null, null, "siti@univ.ac.id", "Dr. Siti Rahayu, M.Sc.", "admin123", 3, 4, null },
                    { 9, null, null, "bmn@univ.ac.id", "Abel Thareq", "admin123", 4, 4, null },
                    { 10, null, null, "kurnadi@univ.ac.id", "Kurnadi", "admin123", 5, 4, null },
                    { 11, null, null, "akmal@univ.ac.id", "Akmal Hasan", "admin123", 4, 4, null }
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
                name: "IX_JadwalEvents_user_id",
                table: "JadwalEvents",
                column: "user_id");

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
                name: "IX_Pengajuan_approved_by",
                table: "Pengajuan",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_id_pejabat",
                table: "Pengajuan",
                column: "id_pejabat");

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_reviewed_by",
                table: "Pengajuan",
                column: "reviewed_by");

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
                name: "JadwalEvents");

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
