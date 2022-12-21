using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBlog.Data.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_BlogUserId",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "BlogUserId",
                table: "BlogPosts",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPosts_BlogUserId",
                table: "BlogPosts",
                newName: "IX_BlogPosts_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_CreatorId",
                table: "BlogPosts",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_CreatorId",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "BlogPosts",
                newName: "BlogUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPosts_CreatorId",
                table: "BlogPosts",
                newName: "IX_BlogPosts_BlogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_BlogUserId",
                table: "BlogPosts",
                column: "BlogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
