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
                name: "FK_ForumComment_User_UserGuid",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_User_UserGuid",
                table: "ForumPost");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "User",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "ForumPost",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPost_UserGuid",
                table: "ForumPost",
                newName: "IX_ForumPost_UserId");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "ForumComment",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_UserGuid",
                table: "ForumComment",
                newName: "IX_ForumComment_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_User_UserId",
                table: "ForumComment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPost_User_UserId",
                table: "ForumPost",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_User_UserId",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_User_UserId",
                table: "ForumPost");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ForumPost",
                newName: "UserGuid");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPost_UserId",
                table: "ForumPost",
                newName: "IX_ForumPost_UserGuid");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ForumComment",
                newName: "UserGuid");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_UserId",
                table: "ForumComment",
                newName: "IX_ForumComment_UserGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_User_UserGuid",
                table: "ForumComment",
                column: "UserGuid",
                principalTable: "User",
                principalColumn: "UserGuid",
                onDelete: ReferentialAction.Restrict);

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
