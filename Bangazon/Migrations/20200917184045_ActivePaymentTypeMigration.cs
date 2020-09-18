using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class ActivePaymentTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActivePayment",
                table: "PaymentType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "23a96448-d5e3-47ed-aed8-f6c852db4fe7", "AQAAAAEAACcQAAAAEPP5fuYQ9b/36CnZve6qrSntYyinemAAk380ATlQOYd/s+ZsgZr6SCwHxHpWlDXCUA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePayment",
                table: "PaymentType");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cc6c75d8-e43d-4cff-a9c0-e49b7f53d87a", "AQAAAAEAACcQAAAAEJBKyz14BzONDlXlQt4V4KBKyx+s+NmmiHowRYky3K7vlVNxmDxkrYRqTu2gRrrgqg==" });
        }
    }
}
