using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedGroupRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.CreateTable(
                name: "GroupMember",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => new { x.MemberId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupMember_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_MemberRole_MemberRoleId",
                        column: x => x.MemberRoleId,
                        principalTable: "MemberRole",
                        principalColumn: "MemberRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMember_User_MemberId",
                        column: x => x.MemberId,
                        principalTable: "User",
                        principalColumn: "UserGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MemberRole",
                columns: new[] { "MemberRoleId", "Description", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000010"), "Is a member of a group", "GroupMember" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupId",
                table: "GroupMember",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_MemberRoleId",
                table: "GroupMember",
                column: "MemberRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMember");

            migrationBuilder.DeleteData(
                table: "MemberRole",
                keyColumn: "MemberRoleId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => new { x.MemberId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupMembers_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembers_MemberRole_MemberRoleId",
                        column: x => x.MemberRoleId,
                        principalTable: "MemberRole",
                        principalColumn: "MemberRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMembers_User_MemberId",
                        column: x => x.MemberId,
                        principalTable: "User",
                        principalColumn: "UserGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId",
                table: "GroupMembers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_MemberRoleId",
                table: "GroupMembers",
                column: "MemberRoleId");
        }
    }
}
