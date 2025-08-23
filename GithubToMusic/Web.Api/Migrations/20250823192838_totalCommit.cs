using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GithubCommitsToMusic.Migrations
{
    /// <inheritdoc />
    public partial class totalCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalCommit",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCommit",
                table: "Users");
        }
    }
}
