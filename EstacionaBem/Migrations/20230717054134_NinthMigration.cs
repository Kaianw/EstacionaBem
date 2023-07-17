using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EstacionaBem.Migrations
{
    public partial class NinthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parkings");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.CreateTable(
                name: "estadias",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    placa = table.Column<string>(type: "TEXT", nullable: false),
                    chegada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    saida = table.Column<DateTime>(type: "TEXT", nullable: false),
                    duracao = table.Column<string>(type: "TEXT", nullable: true),
                    preco = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estadias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "precos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    vigenciaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    vigenciaFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    precoHoraInicial = table.Column<float>(type: "REAL", nullable: false),
                    precoHoraAdicional = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_precos", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "estadias");

            migrationBuilder.DropTable(
                name: "precos");

            migrationBuilder.CreateTable(
                name: "Parkings",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    chegada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    duracao = table.Column<string>(type: "TEXT", nullable: true),
                    placa = table.Column<string>(type: "TEXT", nullable: false),
                    preco = table.Column<float>(type: "REAL", nullable: false),
                    saida = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parkings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    precoHoraAdicional = table.Column<float>(type: "REAL", nullable: false),
                    precoHoraInicial = table.Column<float>(type: "REAL", nullable: false),
                    vigenciaFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    vigenciaInicio = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.id);
                });
        }
    }
}
