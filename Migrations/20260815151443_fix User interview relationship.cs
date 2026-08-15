using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplyTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class fixUserinterviewrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Users_UserId",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_UserId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Interviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Interviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_UserId",
                table: "Interviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Users_UserId",
                table: "Interviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
