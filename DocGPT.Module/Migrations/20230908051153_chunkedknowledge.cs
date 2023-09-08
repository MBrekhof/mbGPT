using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocGPT.Module.Migrations
{
    /// <inheritdoc />
    public partial class ChunkedKnowledge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_usedknowledge_chat_chatid",
                table: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "chunksize",
                table: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "fileid",
                table: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "filename",
                table: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "filesize",
                table: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "id",
                table: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "realfilename",
                table: "usedknowledge");

            migrationBuilder.AlterColumn<int>(
                name: "chatid",
                table: "usedknowledge",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_usedknowledge_chat_chatid",
                table: "usedknowledge",
                column: "chatid",
                principalTable: "chat",
                principalColumn: "chatid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_usedknowledge_chat_chatid",
                table: "usedknowledge");

            migrationBuilder.AlterColumn<int>(
                name: "chatid",
                table: "usedknowledge",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "chunksize",
                table: "usedknowledge",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "fileid",
                table: "usedknowledge",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "filename",
                table: "usedknowledge",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "filesize",
                table: "usedknowledge",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "usedknowledge",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "realfilename",
                table: "usedknowledge",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_usedknowledge_chat_chatid",
                table: "usedknowledge",
                column: "chatid",
                principalTable: "chat",
                principalColumn: "chatid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
