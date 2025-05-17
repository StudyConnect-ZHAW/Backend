using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForumLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        principalColumn: "UserId");
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumLike");
        }
    }
}
