using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GithubCommitsToMusic.Migrations
{
    /// <inheritdoc />
    public partial class inital2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sheets",
                columns: new[] { "Id", "Name", "VirtualPath" },
                values: new object[,]
                {
                    { 1, "Do", "/Sheets/Do.MP3" },
                    { 2, "Re", "/Sheets/Re.MP3" },
                    { 3, "Mi", "/Sheets/Mi.MP3" },
                    { 4, "Fa", "/Sheets/Fa.MP3" },
                    { 5, "Sol", "/Sheets/Sol.MP3" },
                    { 6, "La", "/Sheets/La.MP3" },
                    { 7, "Si", "/Sheets/Si.MP3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sheets",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
