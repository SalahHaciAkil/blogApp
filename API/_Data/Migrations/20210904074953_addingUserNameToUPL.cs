using Microsoft.EntityFrameworkCore.Migrations;

namespace API._Data.Migrations
{
    public partial class addingUserNameToUPL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersPostLikes_Users_AppUserId",
                table: "UsersPostLikes");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UsersPostLikes",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "PostrName",
                table: "UsersPostLikes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPostLikes_Users_UserId",
                table: "UsersPostLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersPostLikes_Users_UserId",
                table: "UsersPostLikes");

            migrationBuilder.DropColumn(
                name: "PostrName",
                table: "UsersPostLikes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UsersPostLikes",
                newName: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPostLikes_Users_AppUserId",
                table: "UsersPostLikes",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
