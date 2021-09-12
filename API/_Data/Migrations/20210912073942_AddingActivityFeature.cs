using Microsoft.EntityFrameworkCore.Migrations;

namespace API._Data.Migrations
{
    public partial class AddingActivityFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "PostrId",
                table: "UsersPostLikes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "UsersPostLikes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PostrId",
                table: "UsersPostComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PostrName",
                table: "UsersPostComments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "UsersPostComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "PostrId",
                table: "UsersPostLikes");

            migrationBuilder.DropColumn(
                name: "Read",
                table: "UsersPostLikes");

            migrationBuilder.DropColumn(
                name: "PostrId",
                table: "UsersPostComments");

            migrationBuilder.DropColumn(
                name: "PostrName",
                table: "UsersPostComments");

            migrationBuilder.DropColumn(
                name: "Read",
                table: "UsersPostComments");
        }
    }
}
