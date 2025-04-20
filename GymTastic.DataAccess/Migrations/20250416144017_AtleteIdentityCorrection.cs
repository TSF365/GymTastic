using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTastic.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AtleteIdentityCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atletes_AspNetUsers_UserId",
                table: "Atletes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Atletes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Atletes_AspNetUsers_UserId",
                table: "Atletes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atletes_AspNetUsers_UserId",
                table: "Atletes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Atletes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Atletes_AspNetUsers_UserId",
                table: "Atletes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
