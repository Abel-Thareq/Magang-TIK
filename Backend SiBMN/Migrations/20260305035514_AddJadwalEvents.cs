using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiBMN.Migrations
{
    /// <inheritdoc />
    public partial class AddJadwalEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_JadwalEvents_user_id",
                table: "JadwalEvents",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JadwalEvents");
        }
    }
}
