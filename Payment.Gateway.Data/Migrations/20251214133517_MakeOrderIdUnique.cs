using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeOrderIdUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_OrderId",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderId",
                table: "Transactions",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_OrderId",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderId",
                table: "Transactions",
                column: "OrderId");
        }
    }
}
