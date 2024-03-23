using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pix.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_CPF",
                table: "Users",
                column: "CPF");

            migrationBuilder.CreateIndex(
                name: "IX_Key_Type_Value",
                table: "Keys",
                columns: new[] { "Type", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_Bank_Token",
                table: "Banks",
                column: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_CPF",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Key_Type_Value",
                table: "Keys");

            migrationBuilder.DropIndex(
                name: "IX_Bank_Token",
                table: "Banks");
        }
    }
}
