using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSClubInsights.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUniveristyAndCollegeTableAndRelateThemToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "collegeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "univeristyId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "College",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_College", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Univeristy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Univeristy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_collegeId",
                table: "AspNetUsers",
                column: "collegeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_univeristyId",
                table: "AspNetUsers",
                column: "univeristyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_College_collegeId",
                table: "AspNetUsers",
                column: "collegeId",
                principalTable: "College",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Univeristy_univeristyId",
                table: "AspNetUsers",
                column: "univeristyId",
                principalTable: "Univeristy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_College_collegeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Univeristy_univeristyId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "College");

            migrationBuilder.DropTable(
                name: "Univeristy");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_collegeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_univeristyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "collegeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "univeristyId",
                table: "AspNetUsers");
        }
    }
}
