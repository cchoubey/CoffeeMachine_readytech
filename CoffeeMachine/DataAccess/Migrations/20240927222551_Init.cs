using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeMachine.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoffeeStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RefillDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeStocks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CoffeeStocks",
                columns: new[] { "Id", "Quantity", "RefillDate" },
                values: new object[] { 1, 4, new DateTimeOffset(new DateTime(2024, 9, 28, 10, 25, 49, 761, DateTimeKind.Unspecified).AddTicks(157), new TimeSpan(0, 12, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeStocks");
        }
    }
}
