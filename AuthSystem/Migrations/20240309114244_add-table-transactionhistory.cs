using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTest_Clicknext.Migrations
{
    /// <inheritdoc />
    public partial class addtabletransactionhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionsHistorys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionUniqueReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionAmount = table.Column<double>(type: "float", nullable: false),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false),
                    TransactionSourceAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDestinationAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionParticulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionSourceAccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDestinationAccountName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionsHistorys", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionsHistorys");
        }
    }
}
