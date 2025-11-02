using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoVet.Migrations
{
    /// <inheritdoc />
    public partial class AjustesModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Veterinario_DataAdmissao",
                table: "Pessoas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Veterinario_Salario",
                table: "Pessoas",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Veterinario_DataAdmissao",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Veterinario_Salario",
                table: "Pessoas");
        }
    }
}
