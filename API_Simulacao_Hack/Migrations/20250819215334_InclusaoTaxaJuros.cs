using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace API_Simulacao_Hack.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class InclusaoTaxaJuros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "taxaJuros",
                table: "Simulacoes",
                type: "decimal(10,9)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "taxaJuros",
                table: "Simulacoes");
        }
    }
}
