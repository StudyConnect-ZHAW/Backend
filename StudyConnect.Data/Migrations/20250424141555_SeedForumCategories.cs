using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedForumCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ForumCategory",
                keyColumn: "ForumCategoryId",
                keyValue: new Guid("c345e8a7-8c49-4326-83e7-2657b1d149f3"),
                column: "Description",
                value: "Betriebssysteme");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ForumCategory",
                keyColumn: "ForumCategoryId",
                keyValue: new Guid("c345e8a7-8c49-4326-83e7-2657b1d149f3"),
                column: "Description",
                value: "Betriebssysteme");
        }
    }
}
