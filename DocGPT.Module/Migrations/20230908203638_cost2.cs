using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocGPT.Module.Migrations
{
    /// <inheritdoc />
    public partial class cost2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cost",
                columns: table => new
                {
                    costid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    action = table.Column<int>(type: "integer", nullable: true),
                    sourcetype = table.Column<int>(type: "integer", nullable: true),
                    articledetailid = table.Column<int>(type: "integer", nullable: true),
                    llmaction = table.Column<int>(type: "integer", nullable: true),
                    codeobjectid = table.Column<int>(type: "integer", nullable: true),
                    chatid = table.Column<int>(type: "integer", nullable: true),
                    prompttokens = table.Column<int>(type: "integer", nullable: true),
                    completiontokens = table.Column<int>(type: "integer", nullable: true),
                    totaltokens = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cost", x => x.costid);
                    table.ForeignKey(
                        name: "fk_cost_articledetail_articledetailid",
                        column: x => x.articledetailid,
                        principalTable: "articledetail",
                        principalColumn: "articledetailid");
                    table.ForeignKey(
                        name: "fk_cost_chat_chatid",
                        column: x => x.chatid,
                        principalTable: "chat",
                        principalColumn: "chatid");
                    table.ForeignKey(
                        name: "fk_cost_codeobject_codeobjectid",
                        column: x => x.codeobjectid,
                        principalTable: "codeobject",
                        principalColumn: "codeobjectid");
                });

            migrationBuilder.CreateIndex(
                name: "ix_cost_articledetailid",
                table: "cost",
                column: "articledetailid");

            migrationBuilder.CreateIndex(
                name: "ix_cost_chatid",
                table: "cost",
                column: "chatid");

            migrationBuilder.CreateIndex(
                name: "ix_cost_codeobjectid",
                table: "cost",
                column: "codeobjectid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cost");
        }
    }
}
