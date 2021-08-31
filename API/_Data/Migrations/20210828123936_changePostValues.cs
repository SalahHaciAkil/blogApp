using Microsoft.EntityFrameworkCore.Migrations;

namespace API._Data.Migrations
{
    public partial class changePostValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Posts",
                newName: "PostTitle");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "PostContent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostTitle",
                table: "Posts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "PostContent",
                table: "Posts",
                newName: "Content");
        }
    }
}
