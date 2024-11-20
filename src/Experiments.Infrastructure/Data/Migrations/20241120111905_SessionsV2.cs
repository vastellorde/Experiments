using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Experiments.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SessionsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "Sessions");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RefreshTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "AuthConfirmations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "AuthConfirmations");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
