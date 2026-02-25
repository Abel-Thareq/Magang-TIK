using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SiBMN.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    spesifikasi = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail_Pengajuan", x => x.id_detPengajuan);
                    table.ForeignKey(
                        name: "FK_Detail_Pengajuan_Master_Barang_id_barang",
                        column: x => x.id_barang,
                        principalTable: "Master_Barang",
                        principalColumn: "id_barang",
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
                table: "Roles",
                columns: new[] { "id_role", "nama_role" },
                values: new object[,]
                {
                    { 1, "Admin Unit Kerja" },
                    { 2, "Tim Pengadaan" },
                    { 3, "Admin Pusat" }
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
                    { 5, "siti@univ.ac.id", "Dr. Siti Rahayu, M.Sc.", "admin123", 3, 4 }
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
