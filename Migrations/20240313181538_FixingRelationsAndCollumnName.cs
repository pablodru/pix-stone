using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pix.Migrations
{
    /// <inheritdoc />
    public partial class FixingRelationsAndCollumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Accounts_AccountId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Banks_BankId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Keys_KeyId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_BankId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_KeyId",
                table: "Payments",
                newName: "IX_Payments_KeyId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_AccountId",
                table: "Payments",
                newName: "IX_Payments_AccountId");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Accounts_AccountId",
                table: "Payments",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Keys_KeyId",
                table: "Payments",
                column: "KeyId",
                principalTable: "Keys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Accounts_AccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Keys_KeyId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_KeyId",
                table: "Payment",
                newName: "IX_Payment_KeyId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_AccountId",
                table: "Payment",
                newName: "IX_Payment_AccountId");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Payment",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "Payment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BankId",
                table: "Payment",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Accounts_AccountId",
                table: "Payment",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Banks_BankId",
                table: "Payment",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Keys_KeyId",
                table: "Payment",
                column: "KeyId",
                principalTable: "Keys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
