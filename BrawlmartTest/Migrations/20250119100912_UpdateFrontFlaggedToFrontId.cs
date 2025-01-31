using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrawlmartTest.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFrontFlaggedToFrontId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FrontFlagged",
                table: "Products",
                newName: "FrontId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FrontId",
                table: "Products",
                newName: "FrontFlagged");
        }
    }
}
