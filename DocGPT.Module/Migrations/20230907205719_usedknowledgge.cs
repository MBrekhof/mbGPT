﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocGPT.Module.Migrations
{
    /// <inheritdoc />
    public partial class usedknowledgge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
                //name: "promptbody",
                //table: "prompt",
                //newName: "systemprompt");

            //migrationBuilder.AddColumn<char>(
            //    name: "articletype",
            //    table: "similarcontentarticlesresult",
            //    type: "character(1)",
            //    nullable: false,
            //    defaultValue: ' ');

            //migrationBuilder.AddColumn<string>(
            //    name: "assistantprompt",
            //    table: "prompt",
            //    type: "text",
            //    unicode: false,
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "usedknowledge",
                columns: table => new
                {
                    usedknowledgeid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    filename = table.Column<string>(type: "text", nullable: true),
                    realfilename = table.Column<string>(type: "text", nullable: true),
                    fileid = table.Column<Guid>(type: "uuid", nullable: true),
                    filesize = table.Column<int>(type: "integer", nullable: true),
                    chunksize = table.Column<int>(type: "integer", nullable: false),
                    chatid = table.Column<int>(type: "integer", nullable: false),
                    articledetailid = table.Column<int>(type: "integer", nullable: true),
                    codeobjectid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usedknowledge", x => x.usedknowledgeid);
                    table.ForeignKey(
                        name: "fk_usedknowledge_articledetail_articledetailid",
                        column: x => x.articledetailid,
                        principalTable: "articledetail",
                        principalColumn: "articledetailid");
                    table.ForeignKey(
                        name: "fk_usedknowledge_chat_chatid",
                        column: x => x.chatid,
                        principalTable: "chat",
                        principalColumn: "chatid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usedknowledge_codeobject_codeobjectid",
                        column: x => x.codeobjectid,
                        principalTable: "codeobject",
                        principalColumn: "codeobjectid");
                });

            migrationBuilder.CreateIndex(
                name: "ix_usedknowledge_articledetailid",
                table: "usedknowledge",
                column: "articledetailid");

            migrationBuilder.CreateIndex(
                name: "ix_usedknowledge_chatid",
                table: "usedknowledge",
                column: "chatid");

            migrationBuilder.CreateIndex(
                name: "ix_usedknowledge_codeobjectid",
                table: "usedknowledge",
                column: "codeobjectid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "articletype",
                table: "similarcontentarticlesresult");

            migrationBuilder.DropColumn(
                name: "assistantprompt",
                table: "prompt");

            migrationBuilder.RenameColumn(
                name: "systemprompt",
                table: "prompt",
                newName: "promptbody");
        }
    }
}
