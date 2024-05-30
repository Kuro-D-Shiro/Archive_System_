using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archive_System.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueDocuments",
                table: "IssueDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "IssueDocuments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueDocuments",
                table: "IssueDocuments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DocumentSubscriber",
                columns: table => new
                {
                    DocumentsId = table.Column<int>(type: "int", nullable: false),
                    SubscribersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSubscriber", x => new { x.DocumentsId, x.SubscribersId });
                    table.ForeignKey(
                        name: "FK_DocumentSubscriber_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentSubscriber_Subscribers_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueDocuments_SubscriberId",
                table: "IssueDocuments",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSubscriber_SubscribersId",
                table: "DocumentSubscriber",
                column: "SubscribersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSubscriber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueDocuments",
                table: "IssueDocuments");

            migrationBuilder.DropIndex(
                name: "IX_IssueDocuments_SubscriberId",
                table: "IssueDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "IssueDocuments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueDocuments",
                table: "IssueDocuments",
                columns: new[] { "SubscriberId", "DocumentId" });
        }
    }
}
