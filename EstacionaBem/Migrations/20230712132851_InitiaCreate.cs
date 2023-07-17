using Microsoft.EntityFrameworkCore.Migrations;

namespace EstacionaBem.Migrations
{
    public partial class InitiaCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parkings",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    placa = table.Column<string>(type: "TEXT", nullable: true),
                    chegada = table.Column<string>(type: "TEXT", nullable: true),
                    saida = table.Column<string>(type: "TEXT", nullable: true),
                    preco = table.Column<string>(type: "TEXT", nullable: true)
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
                    vigenciaInicio = table.Column<string>(type: "TEXT", nullable: true),
                    vigenciaFim = table.Column<string>(type: "TEXT", nullable: true),
                    precoHoraInicial = table.Column<string>(type: "TEXT", nullable: true),
                    precoHoraAdicional = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parkings");

            migrationBuilder.DropTable(
                name: "Prices");
        }
    }
}
