using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SiBMN.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiLevelApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "kabag_umum_approved_at",
                table: "Pengajuan",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "kabag_umum_approved_by",
                table: "Pengajuan",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "kabiro_bpku_approved_at",
                table: "Pengajuan",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "kabiro_bpku_approved_by",
                table: "Pengajuan",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pimpinan_unit_approved_at",
                table: "Pengajuan",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "pimpinan_unit_approved_by",
                table: "Pengajuan",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "submitted_at",
                table: "Pengajuan",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "wr_bpku_approved_at",
                table: "Pengajuan",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "wr_bpku_approved_by",
                table: "Pengajuan",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 1,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 2,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 3,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 4,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 5,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 6,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 7,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 8,
                columns: new[] { "kabag_umum_approved_at", "kabag_umum_approved_by", "kabiro_bpku_approved_at", "kabiro_bpku_approved_by", "pimpinan_unit_approved_at", "pimpinan_unit_approved_by", "submitted_at", "wr_bpku_approved_at", "wr_bpku_approved_by" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id_role", "created_at", "deleted_at", "nama_role", "updated_at" },
                values: new object[,]
                {
                    { 6, null, null, "Pimpinan Unit Kerja", null },
                    { 7, null, null, "WR BPKU", null },
                    { 8, null, null, "Kabiro BPKU", null },
                    { 9, null, null, "Kabag Umum", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id_user", "created_at", "deleted_at", "email", "nama", "password", "role_id", "unit_id", "updated_at" },
                values: new object[,]
                {
                    { 12, null, null, "pimpinan.upatik@univ.ac.id", "Pimpinan UPA TIK", "admin123", 6, 1, null },
                    { 13, null, null, "pimpinan.upabahasa@univ.ac.id", "Pimpinan UPA Bahasa", "admin123", 6, 2, null },
                    { 14, null, null, "pimpinan.upaperpus@univ.ac.id", "Pimpinan UPA Perpustakaan", "admin123", 6, 3, null },
                    { 15, null, null, "wrbpku@univ.ac.id", "WR BPKU", "admin123", 7, 4, null },
                    { 16, null, null, "kabirobpku@univ.ac.id", "Kabiro BPKU", "admin123", 8, 4, null },
                    { 17, null, null, "kabagumum@univ.ac.id", "Kabag Umum", "admin123", 9, 4, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_kabag_umum_approved_by",
                table: "Pengajuan",
                column: "kabag_umum_approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_kabiro_bpku_approved_by",
                table: "Pengajuan",
                column: "kabiro_bpku_approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_pimpinan_unit_approved_by",
                table: "Pengajuan",
                column: "pimpinan_unit_approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pengajuan_wr_bpku_approved_by",
                table: "Pengajuan",
                column: "wr_bpku_approved_by");

            migrationBuilder.AddForeignKey(
                name: "FK_Pengajuan_Users_kabag_umum_approved_by",
                table: "Pengajuan",
                column: "kabag_umum_approved_by",
                principalTable: "Users",
                principalColumn: "id_user",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pengajuan_Users_kabiro_bpku_approved_by",
                table: "Pengajuan",
                column: "kabiro_bpku_approved_by",
                principalTable: "Users",
                principalColumn: "id_user",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pengajuan_Users_pimpinan_unit_approved_by",
                table: "Pengajuan",
                column: "pimpinan_unit_approved_by",
                principalTable: "Users",
                principalColumn: "id_user",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pengajuan_Users_wr_bpku_approved_by",
                table: "Pengajuan",
                column: "wr_bpku_approved_by",
                principalTable: "Users",
                principalColumn: "id_user",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pengajuan_Users_kabag_umum_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropForeignKey(
                name: "FK_Pengajuan_Users_kabiro_bpku_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropForeignKey(
                name: "FK_Pengajuan_Users_pimpinan_unit_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropForeignKey(
                name: "FK_Pengajuan_Users_wr_bpku_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropIndex(
                name: "IX_Pengajuan_kabag_umum_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropIndex(
                name: "IX_Pengajuan_kabiro_bpku_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropIndex(
                name: "IX_Pengajuan_pimpinan_unit_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropIndex(
                name: "IX_Pengajuan_wr_bpku_approved_by",
                table: "Pengajuan");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id_user",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id_user",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id_user",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id_user",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id_user",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id_user",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "id_role",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "id_role",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "id_role",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "id_role",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "kabag_umum_approved_at",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "kabag_umum_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "kabiro_bpku_approved_at",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "kabiro_bpku_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "pimpinan_unit_approved_at",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "pimpinan_unit_approved_by",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "submitted_at",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "wr_bpku_approved_at",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "wr_bpku_approved_by",
                table: "Pengajuan");
        }
    }
}
