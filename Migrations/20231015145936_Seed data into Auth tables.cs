using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library.Migrations
{
    public partial class SeeddataintoAuthtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("dd656a6d-0861-4306-aaa6-33e9cefc753e"), new Guid("e20bf530-11e8-424b-9844-d10bb180aeff") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("dd656a6d-0861-4306-aaa6-33e9cefc753e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e20bf530-11e8-424b-9844-d10bb180aeff"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "RoleDescription" },
                values: new object[] { new Guid("3a6fbaee-39a3-4f58-a4a4-abddea509c85"), "e1d1649c-6edb-4c5a-820c-7b1c1a6e5fdd", "Admin", "ADMIN", "Quản trị viên" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("9d6d1512-497b-4b25-a950-1f6042022e62"), 0, "ce2b8ec8-0f64-4693-9d18-06e37fb636b9", new DateTime(2023, 10, 15, 21, 59, 35, 730, DateTimeKind.Local).AddTicks(2440), new DateOnly(2002, 7, 2), "admin@admin.com", true, "Nguyễn Ngọc Anh", "Tuấn", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEEnqe7HpPfA33weWXmvYSQ8n/8wocPTgg2kSSUF/aQP7PD7bLNYlDqWwLGITRCvcsA==", "0123456789", false, "https://i.imgur.com/6NQ1n0V.png", null, false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("3a6fbaee-39a3-4f58-a4a4-abddea509c85"), new Guid("9d6d1512-497b-4b25-a950-1f6042022e62") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3a6fbaee-39a3-4f58-a4a4-abddea509c85"), new Guid("9d6d1512-497b-4b25-a950-1f6042022e62") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3a6fbaee-39a3-4f58-a4a4-abddea509c85"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9d6d1512-497b-4b25-a950-1f6042022e62"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "RoleDescription" },
                values: new object[] { new Guid("dd656a6d-0861-4306-aaa6-33e9cefc753e"), "962d7f8c-4267-4835-bbe3-a2925a323c5c", "Admin", "ADMIN", "Quản trị viên" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("e20bf530-11e8-424b-9844-d10bb180aeff"), 0, "e7717c67-6d86-4e43-a888-7890f39ae566", new DateTime(2023, 10, 15, 21, 46, 13, 254, DateTimeKind.Local).AddTicks(1420), new DateOnly(2002, 7, 2), "admin@admin.com", true, "Nguyễn Ngọc Anh", "Tuấn", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", null, "0123456789", false, "https://i.imgur.com/6NQ1n0V.png", null, false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("dd656a6d-0861-4306-aaa6-33e9cefc753e"), new Guid("e20bf530-11e8-424b-9844-d10bb180aeff") });
        }
    }
}
