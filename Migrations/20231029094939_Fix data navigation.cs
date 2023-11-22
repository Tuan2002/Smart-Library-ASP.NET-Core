using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library.Migrations
{
    public partial class Fixdatanavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "41533e67-30e6-44e0-90bc-6b0ed3cbd347", "4ac63404-ba33-41b0-b551-352830aac62f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41533e67-30e6-44e0-90bc-6b0ed3cbd347");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4ac63404-ba33-41b0-b551-352830aac62f");

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: "382f2854-02fc-4fd0-89cc-6876ebb61edf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "71482c1f-dfe9-4477-934f-575677201be1", "1d72f4f0-fdb2-4073-822a-f3e328b46a83", "Quản trị viên", "QUẢN TRỊ VIÊN" });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "CreatedAt", "WorkspaceName" },
                values: new object[] { "828f50fd-0f6f-4c19-84f3-985afdf33632", new DateTime(2023, 10, 29, 16, 49, 39, 517, DateTimeKind.Local).AddTicks(4960), "Viện KT và CN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName", "WorkspaceId" },
                values: new object[] { "57e2a957-45f4-4f10-964e-71bfcc2f6d57", 0, "Vinh, Nghệ An", "e5b238f0-694e-4923-9a6b-a34510e87470", new DateTime(2023, 10, 29, 16, 49, 39, 517, DateTimeKind.Local).AddTicks(5010), new DateOnly(2002, 7, 2), "admin@admin.com", true, "Nguyễn Ngọc Anh", "Tuấn", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEEQOCsBXy1Wm7rsWNeDIJy8KdbDgOr9vNc0W/OErMZOE3/fogVhwman7F5xEquL0JQ==", "0123456789", false, "/upload/user-upload/admin.webp", "0a4a14c2-788e-4929-90f0-423cfcc628fc", false, "admin@admin.com", "828f50fd-0f6f-4c19-84f3-985afdf33632" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "71482c1f-dfe9-4477-934f-575677201be1", "57e2a957-45f4-4f10-964e-71bfcc2f6d57" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "71482c1f-dfe9-4477-934f-575677201be1", "57e2a957-45f4-4f10-964e-71bfcc2f6d57" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71482c1f-dfe9-4477-934f-575677201be1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57e2a957-45f4-4f10-964e-71bfcc2f6d57");

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: "828f50fd-0f6f-4c19-84f3-985afdf33632");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "41533e67-30e6-44e0-90bc-6b0ed3cbd347", "0583c25c-ca36-45c2-aeff-99a276cab2e0", "Quản trị viên", "QUẢN TRỊ VIÊN" });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "CreatedAt", "WorkspaceName" },
                values: new object[] { "382f2854-02fc-4fd0-89cc-6876ebb61edf", new DateTime(2023, 10, 29, 15, 59, 39, 846, DateTimeKind.Local).AddTicks(4130), "Viện KT và CN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName", "WorkspaceId" },
                values: new object[] { "4ac63404-ba33-41b0-b551-352830aac62f", 0, "Vinh, Nghệ An", "f5e654ac-2b4d-4e1f-91e5-874ef933aba0", new DateTime(2023, 10, 29, 15, 59, 39, 846, DateTimeKind.Local).AddTicks(4170), new DateOnly(2002, 7, 2), "admin@admin.com", true, "Nguyễn Ngọc Anh", "Tuấn", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEBN1EVTmKx6Sa1OwzPRo/P9ofQmI2rYAenNbQk9vB/eqrDl4+tGU8un8/yL2Rwjmzg==", "0123456789", false, "/upload/user-upload/admin.webp", "b45246d3-2f03-464d-a923-ba224266bda5", false, "admin@admin.com", "382f2854-02fc-4fd0-89cc-6876ebb61edf" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "41533e67-30e6-44e0-90bc-6b0ed3cbd347", "4ac63404-ba33-41b0-b551-352830aac62f" });
        }
    }
}
