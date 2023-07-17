using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EstacionaBem.Migrations
{
    public partial class EleventhMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duracao",
                table: "estadias");

            migrationBuilder.AlterColumn<DateTime>(
                name: "saida",
                table: "estadias",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "saida",
                table: "estadias",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "duracao",
                table: "estadias",
                type: "TEXT",
                nullable: true);
        }
    }
}
