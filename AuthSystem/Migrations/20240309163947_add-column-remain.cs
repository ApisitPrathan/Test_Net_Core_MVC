using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTest_Clicknext.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnremain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TransactionRemain",
                table: "TransactionsHistorys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionRemain",
                table: "TransactionsHistorys");
        }
    }
}
