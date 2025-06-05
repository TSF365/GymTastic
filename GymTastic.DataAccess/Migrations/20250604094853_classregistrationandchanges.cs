using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTastic.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class classregistrationandchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Regatletes",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecialityId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AtleteId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRegistrations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SpecialityId",
                table: "Classes",
                column: "SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Specialities_SpecialityId",
                table: "Classes",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Specialities_SpecialityId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "ClassRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SpecialityId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Regatletes",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "Classes");
        }
    }
}
