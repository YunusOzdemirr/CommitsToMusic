using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GithubCommitsToMusic.Migrations
{
    /// <inheritdoc />
    public partial class pendingchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Do.wav", "/Sheets/Do.wav" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Re.wav", "/Sheets/Re.wav" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Mi.wav", "/Sheets/Mi.wav" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Fa.wav", "/Sheets/Fa.wav" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Sol.wav", "/Sheets/Sol.wav" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "La.wav", "/Sheets/La.wav" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Si.wav", "/Sheets/Si.wav" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Do.MP3", "/Sheets/Do.MP3" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Re.MP3", "/Sheets/Re.MP3" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Mi.MP3", "/Sheets/Mi.MP3" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Fa.MP3", "/Sheets/Fa.MP3" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Sol.MP3", "/Sheets/Sol.MP3" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "La.MP3", "/Sheets/La.MP3" });

            migrationBuilder.UpdateData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "VirtualPath" },
                values: new object[] { "Si.MP3", "/Sheets/Si.MP3" });
        }
    }
}
