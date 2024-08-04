using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mbGPT.Module.Migrations
{
    /// <inheritdoc />
    public partial class created : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<DateTime>(
            //    name: "created",
            //    table: "usedknowledge",
            //    type: "timestamp with time zone",
            //    nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "filesystemstoreobject",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "articledetail",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "article",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created",
                table: "usedknowledge");

            migrationBuilder.DropColumn(
                name: "created",
                table: "filesystemstoreobject");

            migrationBuilder.DropColumn(
                name: "created",
                table: "articledetail");

            migrationBuilder.DropColumn(
                name: "created",
                table: "article");
        }
    }
}
