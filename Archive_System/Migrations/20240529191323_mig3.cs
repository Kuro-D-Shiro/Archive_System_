using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archive_System.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_CellId",
                table: "Documents");

            migrationBuilder.AddColumn<long>(
                name: "InstancedCount",
                table: "IssueDocuments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "CellId",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CellId",
                table: "Documents",
                column: "CellId",
                unique: true,
                filter: "[CellId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents",
                column: "CellId",
                principalTable: "Cells",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_CellId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "InstancedCount",
                table: "IssueDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "CellId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CellId",
                table: "Documents",
                column: "CellId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Cells_CellId",
                table: "Documents",
                column: "CellId",
                principalTable: "Cells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
