using Microsoft.EntityFrameworkCore.Migrations;

namespace V2Capstone.Data.Migrations
{
    public partial class nullableoverall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OverallGrade",
                table: "Student",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OverallGrade",
                table: "Student",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
