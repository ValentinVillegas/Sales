using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.API.Migrations
{
    /// <inheritdoc />
    public partial class PruebaOrdenamientoPaises2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Municipios_EstadoId",
                table: "Municipios");

            migrationBuilder.DropIndex(
                name: "IX_Municipios_Nombre_EstadoId",
                table: "Municipios");

            migrationBuilder.DropIndex(
                name: "IX_Estados_Nombre_PaisId",
                table: "Estados");

            migrationBuilder.DropIndex(
                name: "IX_Estados_PaisId",
                table: "Estados");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_EstadoId_Nombre",
                table: "Municipios",
                columns: new[] { "EstadoId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estados_PaisId_Nombre",
                table: "Estados",
                columns: new[] { "PaisId", "Nombre" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Municipios_EstadoId_Nombre",
                table: "Municipios");

            migrationBuilder.DropIndex(
                name: "IX_Estados_PaisId_Nombre",
                table: "Estados");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_EstadoId",
                table: "Municipios",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_Nombre_EstadoId",
                table: "Municipios",
                columns: new[] { "Nombre", "EstadoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estados_Nombre_PaisId",
                table: "Estados",
                columns: new[] { "Nombre", "PaisId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estados_PaisId",
                table: "Estados",
                column: "PaisId");
        }
    }
}
