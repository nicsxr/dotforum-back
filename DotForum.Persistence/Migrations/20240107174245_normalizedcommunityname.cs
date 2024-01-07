using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotForum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class normalizedcommunityname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Communities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Communities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_Name",
                table: "Communities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_NormalizedName",
                table: "Communities",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Communities_Name",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_NormalizedName",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Communities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
