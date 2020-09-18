using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class databaseErrorFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e34e36a1-70e1-43f3-98c0-141fb3050a74", "AQAAAAEAACcQAAAAED8ecyNPfqe2WfYgbFjsx2nPkHYcn80Aj8xhtXeZTGz7ek3WkJP4cHFgt4wbi5Km3A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cc6c75d8-e43d-4cff-a9c0-e49b7f53d87a", "AQAAAAEAACcQAAAAEJBKyz14BzONDlXlQt4V4KBKyx+s+NmmiHowRYky3K7vlVNxmDxkrYRqTu2gRrrgqg==" });
        }
    }
}
