using Microsoft.EntityFrameworkCore.Migrations;

namespace Kernel.EF.Migrations
{
    public partial class AddUserName2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName2",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName2",
                table: "Users");
        }
    }
}
