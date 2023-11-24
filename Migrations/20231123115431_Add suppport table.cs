using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library.Migrations
{
    public partial class Addsuppporttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyMajor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudyGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportTypeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTickets_SupportTypes_SupportTypeId",
                        column: x => x.SupportTypeId,
                        principalTable: "SupportTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_SupportTypeId",
                table: "SupportTickets",
                column: "SupportTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTickets");

            migrationBuilder.DropTable(
                name: "SupportTypes");
        }
    }
}
