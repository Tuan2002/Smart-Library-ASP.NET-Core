using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library.Migrations
{
    public partial class Addstatusdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "StatusId", "Description", "StatusName" },
                values: new object[,]
                {
                    { 1, "Đơn mượn sách đang chờ xử lý", "Đang chờ xử lý" },
                    { 2, "Đơn đang chờ lấy sách", "Chờ lấy sách" },
                    { 3, "Đã bàn giáo sách cho người mượn", "Đã bàn giao sách" },
                    { 4, "Đã trả sách cho thủ thư", "Đã trả sách" },
                    { 5, "Đơn mượn sách quá hạn trả sách", "Quá hạn trả sách" },
                    { 6, "Đơn mượn sách đã hủy", "Đã hủy" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "StatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "StatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "StatusId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "StatusId",
                keyValue: 6);
        }
    }
}
