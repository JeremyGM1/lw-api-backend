using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RequisitionLogs",
                newName: "UserEmail");

            migrationBuilder.AddColumn<string>(
                name: "ActionResult",
                table: "RequisitionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "RequisitionLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionResult",
                table: "RequisitionLogs");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "RequisitionLogs");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "RequisitionLogs",
                newName: "UserId");
        }
    }
}
