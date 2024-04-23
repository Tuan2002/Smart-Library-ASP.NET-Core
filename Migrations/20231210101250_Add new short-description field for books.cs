using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library.Migrations
{
    public partial class Addnewshortdescriptionfieldforbooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Books");
        }
    }
}
