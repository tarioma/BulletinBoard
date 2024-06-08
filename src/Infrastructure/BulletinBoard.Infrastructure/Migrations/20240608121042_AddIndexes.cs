using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_created_utc",
                table: "users",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_users_is_admin",
                table: "users",
                column: "is_admin");

            migrationBuilder.CreateIndex(
                name: "IX_users_name",
                table: "users",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_bulletins_created_utc",
                table: "bulletins",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_bulletins_expiry_utc",
                table: "bulletins",
                column: "expiry_utc");

            migrationBuilder.CreateIndex(
                name: "IX_bulletins_number",
                table: "bulletins",
                column: "number");

            migrationBuilder.CreateIndex(
                name: "IX_bulletins_rating",
                table: "bulletins",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "IX_bulletins_text",
                table: "bulletins",
                column: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_created_utc",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_is_admin",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_name",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_bulletins_created_utc",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "IX_bulletins_expiry_utc",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "IX_bulletins_number",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "IX_bulletins_rating",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "IX_bulletins_text",
                table: "bulletins");
        }
    }
}
