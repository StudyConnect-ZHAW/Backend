using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupPostComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMember",
                table: "GroupMember");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GroupMember");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupMemberId",
                table: "GroupMember",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMember",
                table: "GroupMember",
                column: "GroupMemberId");

            migrationBuilder.CreateTable(
                name: "GroupPost",
                columns: table => new
                {
                    GroupPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GroupMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPost", x => x.GroupPostId);
                    table.ForeignKey(
                        name: "FK_GroupPost_GroupMember_GroupMemberId",
                        column: x => x.GroupMemberId,
                        principalTable: "GroupMember",
                        principalColumn: "GroupMemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupComment",
                columns: table => new
                {
                    GroupCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupComment", x => x.GroupCommentId);
                    table.ForeignKey(
                        name: "FK_GroupComment_GroupMember_GroupMemberId",
                        column: x => x.GroupMemberId,
                        principalTable: "GroupMember",
                        principalColumn: "GroupMemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupComment_GroupPost_GroupPostId",
                        column: x => x.GroupPostId,
                        principalTable: "GroupPost",
                        principalColumn: "GroupPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_MemberId",
                table: "GroupMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupComment_GroupMemberId",
                table: "GroupComment",
                column: "GroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupComment_GroupPostId",
                table: "GroupComment",
                column: "GroupPostId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPost_GroupMemberId",
                table: "GroupPost",
                column: "GroupMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupComment");

            migrationBuilder.DropTable(
                name: "GroupPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMember",
                table: "GroupMember");

            migrationBuilder.DropIndex(
                name: "IX_GroupMember_MemberId",
                table: "GroupMember");

            migrationBuilder.DropColumn(
                name: "GroupMemberId",
                table: "GroupMember");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GroupMember",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMember",
                table: "GroupMember",
                columns: new[] { "MemberId", "GroupId" });
        }
    }
}
