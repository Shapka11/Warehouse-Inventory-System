using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rolls",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    length = table.Column<double>(type: "double precision", nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    added_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    removed_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolls", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rolls_added_date",
                table: "rolls",
                column: "added_date");

            migrationBuilder.CreateIndex(
                name: "IX_rolls_removed_date",
                table: "rolls",
                column: "removed_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rolls");
        }
    }
}
