using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace V2Capstone.Data.Migrations
{
    public partial class ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Parent",
                columns: table => new
                {
                    ParentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    IsNotified = table.Column<bool>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parent", x => x.ParentId);
                    table.ForeignKey(
                        name: "FK_Parent_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    TeacherId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.TeacherId);
                    table.ForeignKey(
                        name: "FK_Teacher_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(nullable: true),
                    Recipients = table.Column<string>(nullable: true),
                    TeacherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alert_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ScienceGrade = table.Column<double>(nullable: false),
                    MathGrade = table.Column<double>(nullable: false),
                    HistoryGrade = table.Column<double>(nullable: false),
                    MathRank = table.Column<int>(nullable: false),
                    ScienceRank = table.Column<int>(nullable: false),
                    HistoryRank = table.Column<int>(nullable: false),
                    OverallRank = table.Column<int>(nullable: false),
                    OverallGrade = table.Column<double>(nullable: false),
                    GradeLevel = table.Column<int>(nullable: false),
                    IsNotified = table.Column<bool>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Student_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Parent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "ParentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Student_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_TeacherId",
                table: "Alert",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Parent_Id",
                table: "Parent",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Id",
                table: "Student",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ParentId",
                table: "Student",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_TeacherId",
                table: "Student",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Id",
                table: "Teacher",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Parent");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
