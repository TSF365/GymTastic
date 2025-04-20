using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTastic.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AtleteIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Atletes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Atletes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Atletes_UserId",
                table: "Atletes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atletes_AspNetUsers_UserId",
                table: "Atletes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atletes_AspNetUsers_UserId",
                table: "Atletes");

            migrationBuilder.DropIndex(
                name: "IX_Atletes_UserId",
                table: "Atletes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Atletes");

            migrationBuilder.InsertData(
                table: "Atletes",
                columns: new[] { "Id", "Address", "BirthDate", "CC", "City", "Email", "EmergencyContact", "EmergencyEmail", "EmergencyPhone", "FIN", "FirstName", "GenderId", "Height", "InscriptionDate", "LastName", "PhoneNumber", "PhotoUrl", "Weight", "ZipCode" },
                values: new object[] { 1, "Estrada da Póvoa", new DateTime(2007, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "12113131", "Lisbon", "tomasantifernandes@gmail.com", "Tomás", "tomas@gmail.com", "1212121", 2000000, "Tomás", 2, 1.8f, new DateTime(2022, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Santos Fernandes", "999999999", "", 62.4f, "1750-224" });
        }
    }
}
