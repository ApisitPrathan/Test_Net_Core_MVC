using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTest_Clicknext.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumberGenerated",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "CurrentAccountBalance",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumberGenerated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentAccountBalance",
                table: "AspNetUsers");
        }
    }
}
