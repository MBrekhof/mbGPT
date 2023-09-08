using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocGPT.Module.Migrations
{
    /// <inheritdoc />
    public partial class cost3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "action",
                table: "cost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "action",
                table: "cost",
                type: "integer",
                nullable: true);
        }
    }
}
