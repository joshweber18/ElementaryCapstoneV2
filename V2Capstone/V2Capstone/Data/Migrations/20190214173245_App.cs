using Microsoft.EntityFrameworkCore.Migrations;

namespace V2Capstone.Data.Migrations
{
    public partial class App : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ScienceGrade",
                table: "Student",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "MathGrade",
                table: "Student",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "HistoryGrade",
                table: "Student",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "GradeLevel",
                table: "Student",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ScienceGrade",
                table: "Student",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "MathGrade",
                table: "Student",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "HistoryGrade",
                table: "Student",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradeLevel",
                table: "Student",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
