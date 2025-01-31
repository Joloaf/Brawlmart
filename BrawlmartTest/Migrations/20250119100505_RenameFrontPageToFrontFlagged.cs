using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrawlmartTest.Migrations
{
    /// <inheritdoc />
    public partial class RenameFrontPageToFrontFlagged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FrontPage",
                table: "Products",
                newName: "FrontFlagged");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FrontFlagged",
                table: "Products",
                newName: "FrontPage");
        }
    }
}
