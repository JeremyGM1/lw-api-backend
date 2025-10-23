using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RequisitionLogsChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewValues",
                table: "RequisitionLogs");

            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "RequisitionLogs");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "RequisitionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "RequisitionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "RequisitionLogs");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "RequisitionLogs");

            migrationBuilder.AddColumn<string>(
                name: "NewValues",
                table: "RequisitionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "RequisitionLogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
