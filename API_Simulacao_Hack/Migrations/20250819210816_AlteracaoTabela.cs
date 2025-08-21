using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Simulacao_Hack.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "codigoProduto",
                table: "Simulacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "dataReferencia",
                table: "Simulacoes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "descricaoProduto",
                table: "Simulacoes",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigoProduto",
                table: "Simulacoes");

            migrationBuilder.DropColumn(
                name: "dataReferencia",
                table: "Simulacoes");

            migrationBuilder.DropColumn(
                name: "descricaoProduto",
                table: "Simulacoes");
        }
    }
}
