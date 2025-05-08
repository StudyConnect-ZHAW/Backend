using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_ForumComment_ParentCommentId",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "DislikeCount",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "ForumComment");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "ForumComment",
                newName: "ParentCommentForumCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_ParentCommentId",
                table: "ForumComment",
                newName: "IX_ForumComment_ParentCommentForumCommentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserGuid",
                table: "ForumComment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_ForumComment_ParentCommentForumCommentId",
                table: "ForumComment",
                column: "ParentCommentForumCommentId",
                principalTable: "ForumComment",
                principalColumn: "ForumCommentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_ForumComment_ParentCommentForumCommentId",
                table: "ForumComment");

            migrationBuilder.RenameColumn(
                name: "ParentCommentForumCommentId",
                table: "ForumComment",
                newName: "ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_ParentCommentForumCommentId",
                table: "ForumComment",
                newName: "IX_ForumComment_ParentCommentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserGuid",
                table: "ForumComment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "DislikeCount",
                table: "ForumComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "ForumComment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "ForumComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "ForumComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_ForumComment_ParentCommentId",
                table: "ForumComment",
                column: "ParentCommentId",
                principalTable: "ForumComment",
                principalColumn: "ForumCommentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
