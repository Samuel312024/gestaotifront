using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace integracoes.Migrations
{
    /// <inheritdoc />
    public partial class AddCpfToRawData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "RawDatas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "RawDatas");
        }
    }
}
