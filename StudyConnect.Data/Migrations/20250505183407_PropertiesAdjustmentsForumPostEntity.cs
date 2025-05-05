using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class PropertiesAdjustmentsForumPostEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikeCount",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "ForumPost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DislikeCount",
                table: "ForumPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ForumPost",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "ForumPost",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "ForumPost",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "ForumPost",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
