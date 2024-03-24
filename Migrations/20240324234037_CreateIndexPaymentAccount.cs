using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pix.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexPaymentAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_KeyId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payemnt_Idempotence",
                table: "Payments",
                columns: new[] { "KeyId", "AccountId", "Amount" });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Existency",
                table: "Accounts",
                columns: new[] { "Number", "Agency", "BankId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payemnt_Idempotence",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Account_Existency",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_KeyId",
                table: "Payments",
                column: "KeyId");
        }
    }
}
