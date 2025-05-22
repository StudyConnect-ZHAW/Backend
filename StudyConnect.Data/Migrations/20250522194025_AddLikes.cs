using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_ForumComment_ParentCommentForumCommentId",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_User_UserGuid",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_User_UserGuid",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "ForumPost");

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

            migrationBuilder.RenameColumn(
                name: "ParentCommentForumCommentId",
                table: "ForumComment",
                newName: "ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_UserGuid",
                table: "ForumComment",
                newName: "IX_ForumComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_ParentCommentForumCommentId",
                table: "ForumComment",
                newName: "IX_ForumComment_ParentCommentId");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ForumComment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ForumLike",
                columns: table => new
                {
                    LikeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForumPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForumCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumLike", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_ForumLike_ForumComment_ForumCommentId",
                        column: x => x.ForumCommentId,
                        principalTable: "ForumComment",
                        principalColumn: "ForumCommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForumLike_ForumPost_ForumPostId",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPost",
                        principalColumn: "ForumPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumLike_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumLike_ForumCommentId",
                table: "ForumLike",
                column: "ForumCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumLike_ForumPostId",
                table: "ForumLike",
                column: "ForumPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumLike_UserId_ForumPostId_ForumCommentId",
                table: "ForumLike",
                columns: new[] { "UserId", "ForumPostId", "ForumCommentId" },
                unique: true,
                filter: "[ForumPostId] IS NOT NULL AND [ForumCommentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_ForumComment_ParentCommentId",
                table: "ForumComment",
                column: "ParentCommentId",
                principalTable: "ForumComment",
                principalColumn: "ForumCommentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_User_UserId",
                table: "ForumComment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserGuid",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ForumComment_ForumComment_ParentCommentId",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_User_UserId",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_User_UserId",
                table: "ForumPost");

            migrationBuilder.DropTable(
                name: "ForumLike");

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

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "ForumComment",
                newName: "ParentCommentForumCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_UserId",
                table: "ForumComment",
                newName: "IX_ForumComment_UserGuid");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_ParentCommentId",
                table: "ForumComment",
                newName: "IX_ForumComment_ParentCommentForumCommentId");

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "ForumPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ForumComment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_ForumComment_ParentCommentForumCommentId",
                table: "ForumComment",
                column: "ParentCommentForumCommentId",
                principalTable: "ForumComment",
                principalColumn: "ForumCommentId",
                onDelete: ReferentialAction.Restrict);

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
