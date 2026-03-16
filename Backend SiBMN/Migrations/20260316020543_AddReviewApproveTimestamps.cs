using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiBMN.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewApproveTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "approved_at",
                table: "Pengajuan",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "reviewed_at",
                table: "Pengajuan",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 1,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 2,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 3,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 4,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 5,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 6,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 7,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pengajuan",
                keyColumn: "id_pengajuan",
                keyValue: 8,
                columns: new[] { "approved_at", "reviewed_at" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "approved_at",
                table: "Pengajuan");

            migrationBuilder.DropColumn(
                name: "reviewed_at",
                table: "Pengajuan");
        }
    }
}
