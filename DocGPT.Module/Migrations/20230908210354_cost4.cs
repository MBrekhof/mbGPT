using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocGPT.Module.Migrations
{
    /// <inheritdoc />
    public partial class cost4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "articleid",
                table: "cost",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_cost_articleid",
                table: "cost",
                column: "articleid");

            migrationBuilder.AddForeignKey(
                name: "fk_cost_article_articleid",
                table: "cost",
                column: "articleid",
                principalTable: "article",
                principalColumn: "articleid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cost_article_articleid",
                table: "cost");

            migrationBuilder.DropIndex(
                name: "ix_cost_articleid",
                table: "cost");

            migrationBuilder.DropColumn(
                name: "articleid",
                table: "cost");
        }
    }
}
