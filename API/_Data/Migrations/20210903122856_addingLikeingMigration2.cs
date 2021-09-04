using Microsoft.EntityFrameworkCore.Migrations;

namespace API._Data.Migrations
{
    public partial class addingLikeingMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPostLikes_Posts_PostId",
                table: "UserPostLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPostLikes_Users_AppUserId",
                table: "UserPostLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPostLikes",
                table: "UserPostLikes");

            migrationBuilder.RenameTable(
                name: "UserPostLikes",
                newName: "UsersPostLikes");

            migrationBuilder.RenameIndex(
                name: "IX_UserPostLikes_PostId",
                table: "UsersPostLikes",
                newName: "IX_UsersPostLikes_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersPostLikes",
                table: "UsersPostLikes",
                columns: new[] { "AppUserId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPostLikes_Posts_PostId",
                table: "UsersPostLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPostLikes_Users_AppUserId",
                table: "UsersPostLikes",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersPostLikes_Posts_PostId",
                table: "UsersPostLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPostLikes_Users_AppUserId",
                table: "UsersPostLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersPostLikes",
                table: "UsersPostLikes");

            migrationBuilder.RenameTable(
                name: "UsersPostLikes",
                newName: "UserPostLikes");

            migrationBuilder.RenameIndex(
                name: "IX_UsersPostLikes_PostId",
                table: "UserPostLikes",
                newName: "IX_UserPostLikes_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPostLikes",
                table: "UserPostLikes",
                columns: new[] { "AppUserId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostLikes_Posts_PostId",
                table: "UserPostLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPostLikes_Users_AppUserId",
                table: "UserPostLikes",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
