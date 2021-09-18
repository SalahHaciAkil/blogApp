using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API._Data.Migrations
{
    public partial class ExtendingLikePostTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LikedTime",
                table: "UsersPostLikes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserPhoto",
                table: "UsersPostLikes",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedTime",
                table: "UsersPostLikes");

            migrationBuilder.DropColumn(
                name: "UserPhoto",
                table: "UsersPostLikes");
        }
    }
}
