using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Simulacao_Hack.Migrations
{
    /// <inheritdoc />
    public partial class MigrationFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "tipoSimulacao",
                table: "Simulacoes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tipoSimulacao",
                table: "Simulacoes",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
