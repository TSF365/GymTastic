using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTastic.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class minorchanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Regatletes",
                table: "Classes",
                newName: "RegAtletes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegAtletes",
                table: "Classes",
                newName: "Regatletes");
        }
    }
}
