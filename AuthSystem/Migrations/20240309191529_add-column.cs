using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTest_Clicknext.Migrations
{
    /// <inheritdoc />
    public partial class addcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionRemain",
                table: "TransactionsHistorys",
                newName: "TransactionSourceAccountRemain");

            migrationBuilder.AddColumn<double>(
                name: "TransactionDestinationAccountRemain",
                table: "TransactionsHistorys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionDestinationAccountRemain",
                table: "TransactionsHistorys");

            migrationBuilder.RenameColumn(
                name: "TransactionSourceAccountRemain",
                table: "TransactionsHistorys",
                newName: "TransactionRemain");
        }
    }
}
