using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pix.Migrations
{
    /// <inheritdoc />
    public partial class AddingBankWebHook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebHook",
                table: "Banks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebHook",
                table: "Banks");
        }
    }
}
