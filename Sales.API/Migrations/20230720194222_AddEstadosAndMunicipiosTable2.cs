using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadosAndMunicipiosTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estados_Paises_paisId",
                table: "Estados");

            migrationBuilder.DropIndex(
                name: "IX_Estados_paisId",
                table: "Estados");

            migrationBuilder.RenameColumn(
                name: "paisId",
                table: "Estados",
                newName: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Estados_PaisId_Nombre",
                table: "Estados",
                columns: new[] { "PaisId", "Nombre" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estados_Paises_PaisId",
                table: "Estados",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estados_Paises_PaisId",
                table: "Estados");

            migrationBuilder.DropIndex(
                name: "IX_Estados_PaisId_Nombre",
                table: "Estados");

            migrationBuilder.RenameColumn(
                name: "PaisId",
                table: "Estados",
                newName: "paisId");

            migrationBuilder.CreateIndex(
                name: "IX_Estados_paisId",
                table: "Estados",
                column: "paisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estados_Paises_paisId",
                table: "Estados",
                column: "paisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
