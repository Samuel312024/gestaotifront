using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoTI.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPrioridadeChamado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prioridade",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Chamados");
        }
    }
}
