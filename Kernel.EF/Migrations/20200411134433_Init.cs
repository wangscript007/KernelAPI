using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kernel.EF.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 260, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    TaskMask = table.Column<string>(maxLength: 32, nullable: false),
                    RoleFlags = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    Sid = table.Column<byte[]>(maxLength: 85, nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    AuthType = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 260, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles",
                table: "Roles",
                column: "RoleName",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Users",
                table: "Users",
                columns: new[] { "Sid", "UserName", "AuthType" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
