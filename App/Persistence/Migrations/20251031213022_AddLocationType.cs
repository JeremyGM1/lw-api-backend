using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequisitionLocationTypeId",
                table: "Requisitions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequisitionLocationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionLocationType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RequisitionLocationType",
                columns: new[] { "Id", "LocationType" },
                values: new object[,]
                {
                    { 1, "On-Site" },
                    { 2, "Hybrid" },
                    { 3, "Virtual" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_RequisitionLocationTypeId",
                table: "Requisitions",
                column: "RequisitionLocationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitions_RequisitionLocationType_RequisitionLocationTypeId",
                table: "Requisitions",
                column: "RequisitionLocationTypeId",
                principalTable: "RequisitionLocationType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitions_RequisitionLocationType_RequisitionLocationTypeId",
                table: "Requisitions");

            migrationBuilder.DropTable(
                name: "RequisitionLocationType");

            migrationBuilder.DropIndex(
                name: "IX_Requisitions_RequisitionLocationTypeId",
                table: "Requisitions");

            migrationBuilder.DropColumn(
                name: "RequisitionLocationTypeId",
                table: "Requisitions");
        }
    }
}
