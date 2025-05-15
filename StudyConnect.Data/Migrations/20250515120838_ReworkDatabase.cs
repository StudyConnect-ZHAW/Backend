using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReworkDatabase : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_ForumComment_UserGuid",
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
                name: "UserGuid",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "ForumComment");

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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserRole",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "UserRole",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ForumPost",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ForumComment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ForumComment_UserId",
                table: "ForumComment",
                column: "UserId");

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

            migrationBuilder.DropIndex(
                name: "IX_ForumComment_UserId",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ForumComment");

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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserRole",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "UserRole",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ForumPost",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "ForumComment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "ForumComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ForumComment_UserGuid",
                table: "ForumComment",
                column: "UserGuid");

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
