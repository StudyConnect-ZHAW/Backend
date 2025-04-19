using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "URoleId", "Description", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "Student is the default role with no rights.", "Student" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "URoleId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
