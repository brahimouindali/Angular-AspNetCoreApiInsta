using Microsoft.EntityFrameworkCore.Migrations;

namespace InstagramAPI.Migrations
{
    public partial class addConetentToCommentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
