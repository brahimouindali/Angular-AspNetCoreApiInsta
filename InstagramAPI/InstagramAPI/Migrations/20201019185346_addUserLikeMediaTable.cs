using Microsoft.EntityFrameworkCore.Migrations;

namespace InstagramAPI.Migrations
{
    public partial class addUserLikeMediaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nbLike",
                table: "Medias");

            migrationBuilder.CreateTable(
                name: "UserLikeMedias",
                columns: table => new
                {
                    AppUserId = table.Column<string>(nullable: true),
                    MediaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_UserLikeMedias_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLikeMedias_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeMedias_AppUserId",
                table: "UserLikeMedias",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeMedias_MediaId",
                table: "UserLikeMedias",
                column: "MediaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikeMedias");

            migrationBuilder.AddColumn<int>(
                name: "nbLike",
                table: "Medias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
