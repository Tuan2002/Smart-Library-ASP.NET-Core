using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library.Migrations
{
    public partial class Updateworkspacefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Workspaces_WorkspaceId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workspaces",
                table: "Workspaces");

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
                keyColumnType: "nvarchar(450)",
                keyValue: "4da7d3f8-7793-4108-a5d5-16a4fc6888a2");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Workspaces");

            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "Workspaces",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workspaces",
                table: "Workspaces",
                column: "WorkspaceId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "21eb4ed6-60c0-45e6-910a-bec3e19e9a17", "e424bf1c-7f9a-4ae1-be43-dda6bb57ecac", "Quản trị viên", "QUẢN TRỊ VIÊN" });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "WorkspaceId", "CreatedAt", "WorkspaceName" },
                values: new object[] { 1, new DateTime(2023, 11, 9, 9, 47, 56, 959, DateTimeKind.Local).AddTicks(1870), "Viện KT và CN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName", "WorkspaceId" },
                values: new object[] { "e759ea6d-f837-443c-920e-758d0fc62245", 0, "Vinh, Nghệ An", "e72d40c5-8cc6-4638-8f3e-282e43e53665", new DateTime(2023, 11, 9, 9, 47, 56, 959, DateTimeKind.Local).AddTicks(1980), new DateOnly(2002, 7, 2), "admin@admin.com", true, "Nguyễn Ngọc Anh", "Tuấn", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEJgJIsws3voG/t8rDKpc7DC4cMkdiYdxym8jVfA2o5Gdb/Qz5j7Gm4EZJ+TPw/PSbA==", "0123456789", false, "/upload/user-upload/admin.webp", "f8f1dc18-3156-4e6f-aec3-1e132bb24015", false, "admin@admin.com", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "21eb4ed6-60c0-45e6-910a-bec3e19e9a17", "e759ea6d-f837-443c-920e-758d0fc62245" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Workspaces_WorkspaceId",
                table: "AspNetUsers",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "WorkspaceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Workspaces_WorkspaceId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workspaces",
                table: "Workspaces");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "21eb4ed6-60c0-45e6-910a-bec3e19e9a17", "e759ea6d-f837-443c-920e-758d0fc62245" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21eb4ed6-60c0-45e6-910a-bec3e19e9a17");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e759ea6d-f837-443c-920e-758d0fc62245");

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "WorkspaceId",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Workspaces");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Workspaces",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "WorkspaceId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workspaces",
                table: "Workspaces",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Workspaces_WorkspaceId",
                table: "AspNetUsers",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
