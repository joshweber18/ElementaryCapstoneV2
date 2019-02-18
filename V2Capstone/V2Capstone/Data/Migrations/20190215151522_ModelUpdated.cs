using Microsoft.EntityFrameworkCore.Migrations;

namespace V2Capstone.Data.Migrations
{
    public partial class ModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsNotified",
                table: "Student",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsNotified",
                table: "Student",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
