using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    ForumPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.ForumPostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTag_ForumPost_ForumPostId",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPost",
                        principalColumn: "ForumPostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "TagId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("27a0d0a6-9df8-429e-b473-129533d460d5"), "Looking for study group members", "Looking for Group" },
                    { new Guid("46fd2f68-df2d-4a0a-9137-f8556b4f132f"), "Discuss the topic here", "Discussion" },
                    { new Guid("9d2f3e3f-0f58-4d55-8337-84fecd2b84d3"), "Check existing problems or issues", "Issue" },
                    { new Guid("dba25ed0-ea90-4de8-9b73-bedd16d15a5f"), "Ask your question here", "Question" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                table: "PostTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name",
                table: "Tag",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "Tag");
        }
    }
}
