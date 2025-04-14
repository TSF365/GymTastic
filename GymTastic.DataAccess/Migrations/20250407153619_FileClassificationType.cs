using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTastic.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileClassificationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FileClassificationTypes",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "Identificação" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FileClassificationTypes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
