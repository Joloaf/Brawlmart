using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrawlmartTest.Migrations
{
    /// <inheritdoc />
    public partial class FrontPageFlag : Migration
    {
        /// <inheritdoc />
        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.AddColumn<int>(
        //        name: "FrontPage",
        //        table: "Products",
        //        type: "int",
        //        nullable: true);
        //}

        ///// <inheritdoc />
        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropColumn(
        //        name: "FrontPage",
        //        table: "Products");
        //}

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FrontPage",
                table: "Products",
                newName: "FrontFlagged",
                schema: "dbo");
        }
    }
}
