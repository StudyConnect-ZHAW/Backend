using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ForumCategory",
                columns: new[] { "ForumCategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-5678-4f90-abcd-1234567890ef"), "General Kontext", "General" },
                    { new Guid("a3f1d8a5-1b67-4a4f-91c1-9c63c2bde914"), "Software Entwicklung 2", "SWEN1" },
                    { new Guid("c345e8a7-8c49-4326-83e7-2657b1d149f3"), "Betriebssysteme", "BSY" },
                    { new Guid("e5b7f442-06c6-4d7e-b6fa-4f8f2420a6e7"), "Computer Technik 2", "CT-2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ForumCategory",
                keyColumn: "ForumCategoryId",
                keyValue: new Guid("a1b2c3d4-5678-4f90-abcd-1234567890ef"));

            migrationBuilder.DeleteData(
                table: "ForumCategory",
                keyColumn: "ForumCategoryId",
                keyValue: new Guid("a3f1d8a5-1b67-4a4f-91c1-9c63c2bde914"));

            migrationBuilder.DeleteData(
                table: "ForumCategory",
                keyColumn: "ForumCategoryId",
                keyValue: new Guid("c345e8a7-8c49-4326-83e7-2657b1d149f3"));

            migrationBuilder.DeleteData(
                table: "ForumCategory",
                keyColumn: "ForumCategoryId",
                keyValue: new Guid("e5b7f442-06c6-4d7e-b6fa-4f8f2420a6e7"));
        }
    }
}
