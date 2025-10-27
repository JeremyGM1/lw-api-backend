using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LoggingService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionLogs_Requisitions_RequisitionId",
                table: "RequisitionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionLogs_Requisitions_RequisitionId",
                table: "RequisitionLogs",
                column: "RequisitionId",
                principalTable: "Requisitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionLogs_Requisitions_RequisitionId",
                table: "RequisitionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionLogs_Requisitions_RequisitionId",
                table: "RequisitionLogs",
                column: "RequisitionId",
                principalTable: "Requisitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
