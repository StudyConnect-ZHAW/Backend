using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixForumPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_User_UserGuid",
                table: "ForumPost");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "ForumPost",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPost_UserGuid",
                table: "ForumPost",
                newName: "IX_ForumPost_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPost_User_UserId",
                table: "ForumPost",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserGuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_User_UserId",
                table: "ForumPost");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ForumPost",
                newName: "UserGuid");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPost_UserId",
                table: "ForumPost",
                newName: "IX_ForumPost_UserGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPost_User_UserGuid",
                table: "ForumPost",
                column: "UserGuid",
                principalTable: "User",
                principalColumn: "UserGuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
