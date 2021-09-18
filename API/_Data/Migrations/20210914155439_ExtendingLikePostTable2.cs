using Microsoft.EntityFrameworkCore.Migrations;

namespace API._Data.Migrations
{
    public partial class ExtendingLikePostTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UsersPostLikes",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UsersPostLikes");
        }
    }
}
