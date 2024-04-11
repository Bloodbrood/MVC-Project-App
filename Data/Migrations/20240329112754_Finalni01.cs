using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Finalni01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurance_Person_PersonId",
                table: "Insurance");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_AspNetUsers_IdentityUserId",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurance_Person_PersonId",
                table: "Insurance",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_AspNetUsers_IdentityUserId",
                table: "Person",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurance_Person_PersonId",
                table: "Insurance");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_AspNetUsers_IdentityUserId",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurance_Person_PersonId",
                table: "Insurance",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_AspNetUsers_IdentityUserId",
                table: "Person",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
