using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotForum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserCommentReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCommentReactions",
                columns: table => new
                {
                    UserCommentReactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VoteStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommentReactions", x => x.UserCommentReactionId);
                    table.ForeignKey(
                        name: "FK_UserCommentReactions_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCommentReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentReactions_CommentId",
                table: "UserCommentReactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentReactions_UserId",
                table: "UserCommentReactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCommentReactions");
        }
    }
}
