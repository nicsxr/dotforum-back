using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotForum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserPostReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediaUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserPostReactions",
                columns: table => new
                {
                    UserPostReactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    VoteStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPostReactions", x => x.UserPostReactionId);
                    table.ForeignKey(
                        name: "FK_UserPostReactions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPostReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPostReactions_PostId",
                table: "UserPostReactions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostReactions_UserId",
                table: "UserPostReactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPostReactions");

            migrationBuilder.DropColumn(
                name: "MediaUrl",
                table: "Posts");
        }
    }
}
