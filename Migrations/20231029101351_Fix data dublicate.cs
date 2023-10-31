using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library.Migrations
{
    public partial class Fixdatadublicate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "4804626e-9a1c-407a-b7aa-2a96a6326f41", "f9e32a50-b724-4662-8124-9b68ccfb1311", "Quản trị viên", "QUẢN TRỊ VIÊN" });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "CreatedAt", "WorkspaceName" },
                values: new object[] { "4da7d3f8-7793-4108-a5d5-16a4fc6888a2", new DateTime(2023, 10, 29, 17, 13, 50, 679, DateTimeKind.Local).AddTicks(9530), "Viện KT và CN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName", "WorkspaceId" },
                values: new object[] { "a96e7f92-457d-4787-8c93-0ebdddabe8fc", 0, "Vinh, Nghệ An", "a8c22cb2-d3d7-417a-b9c3-29f154ad6467", new DateTime(2023, 10, 29, 17, 13, 50, 679, DateTimeKind.Local).AddTicks(9580), new DateOnly(2002, 7, 2), "admin@admin.com", true, "Nguyễn Ngọc Anh", "Tuấn", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAECSvlQuNnnZtXP2CuUfAY2QTlSOfOCJXw7UElpQ5HB2lehTcPhyn8DkWNg0b65/7SQ==", "0123456789", false, "/upload/user-upload/admin.webp", "e0fc00f8-09cf-42ce-9b93-677a5e972fda", false, "admin@admin.com", "4da7d3f8-7793-4108-a5d5-16a4fc6888a2" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4804626e-9a1c-407a-b7aa-2a96a6326f41", "a96e7f92-457d-4787-8c93-0ebdddabe8fc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4804626e-9a1c-407a-b7aa-2a96a6326f41", "a96e7f92-457d-4787-8c93-0ebdddabe8fc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4804626e-9a1c-407a-b7aa-2a96a6326f41");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a96e7f92-457d-4787-8c93-0ebdddabe8fc");

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: "4da7d3f8-7793-4108-a5d5-16a4fc6888a2");

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
    }
}
