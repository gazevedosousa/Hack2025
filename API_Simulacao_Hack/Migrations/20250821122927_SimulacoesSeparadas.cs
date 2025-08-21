using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Simulacao_Hack.Migrations
{
    /// <inheritdoc />
    public partial class SimulacoesSeparadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Simulacoes",
                table: "Simulacoes");

            migrationBuilder.AlterColumn<int>(
                name: "idSimulacao",
                table: "Simulacoes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Simulacoes",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "tipoSimulacao",
                table: "Simulacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "valorMediaPrestacoes",
                table: "Simulacoes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Simulacoes",
                table: "Simulacoes",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Simulacoes",
                table: "Simulacoes");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Simulacoes");

            migrationBuilder.DropColumn(
                name: "tipoSimulacao",
                table: "Simulacoes");

            migrationBuilder.DropColumn(
                name: "valorMediaPrestacoes",
                table: "Simulacoes");

            migrationBuilder.AlterColumn<int>(
                name: "idSimulacao",
                table: "Simulacoes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Simulacoes",
                table: "Simulacoes",
                column: "idSimulacao");
        }
    }
}
