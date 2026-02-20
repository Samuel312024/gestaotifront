using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace integracoes.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataUpload",
                table: "RawDataDto",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataUpload",
                table: "RawDataDto");
        }
    }
}
