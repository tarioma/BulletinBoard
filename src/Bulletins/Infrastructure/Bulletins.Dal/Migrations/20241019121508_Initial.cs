using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bulletins.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bulletin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    ExpiryUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bulletin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bulletin_CreatedUtc",
                table: "Bulletin",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Bulletin_ExpiryUtc",
                table: "Bulletin",
                column: "ExpiryUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Bulletin_Number",
                table: "Bulletin",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Bulletin_Text",
                table: "Bulletin",
                column: "Text");

            migrationBuilder.CreateIndex(
                name: "IX_Bulletin_UserId",
                table: "Bulletin",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bulletin");
        }
    }
}
