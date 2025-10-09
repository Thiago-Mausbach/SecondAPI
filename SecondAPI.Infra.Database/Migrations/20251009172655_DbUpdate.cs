using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondAPI.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DadosLivrosId",
                table: "Emprestimos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DadosLivrosId",
                table: "Emprestimos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
