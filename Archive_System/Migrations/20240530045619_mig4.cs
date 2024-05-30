using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archive_System.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents",
                column: "CellId",
                principalTable: "Cells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents",
                column: "CellId",
                principalTable: "Cells",
                principalColumn: "Id");
        }
    }
}
