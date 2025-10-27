using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:App/Persistence/Migrations/20251008172120_InitialPersistenceMigration.cs
    public partial class InitialPersistenceMigration : Migration
========
    public partial class AddSupportEntities : Migration
>>>>>>>> feature/F005-requisition-crud:App/Persistence/Migrations/20251027165451_AddSupportEntities.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobDescriptionLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptionLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobDescriptionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionCurrency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionCurrency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionHardware",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hardware = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionHardware", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionInterval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Interval = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionInterval", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionWorkType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionWorkType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    PostingUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KeyResponsabilities = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDescriptions_JobDescriptionLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "JobDescriptionLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobDescriptions_JobDescriptionStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "JobDescriptionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requisitions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    WorkTypeId = table.Column<int>(type: "int", nullable: false),
                    HardwareId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Headcount = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HiringManager = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompensationBandMin = table.Column<decimal>(type: "decimal(20,4)", precision: 20, scale: 4, nullable: false),
                    CompensationBandMax = table.Column<decimal>(type: "decimal(20,4)", precision: 20, scale: 4, nullable: false),
                    CompensationBandCurrencyId = table.Column<int>(type: "int", nullable: false),
                    CompensationBandIntervalId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BackfillName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisitions_RequisitionCurrency_CompensationBandCurrencyId",
                        column: x => x.CompensationBandCurrencyId,
                        principalTable: "RequisitionCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requisitions_RequisitionHardware_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "RequisitionHardware",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requisitions_RequisitionInterval_CompensationBandIntervalId",
                        column: x => x.CompensationBandIntervalId,
                        principalTable: "RequisitionInterval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requisitions_RequisitionLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "RequisitionLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requisitions_RequisitionStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RequisitionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requisitions_RequisitionWorkType_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "RequisitionWorkType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCandidates",
                columns: table => new
                {
                    JobDescriptionId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCandidates", x => new { x.JobDescriptionId, x.CandidateId });
                    table.ForeignKey(
                        name: "FK_JobCandidates_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobCandidates_JobDescriptions_JobDescriptionId",
                        column: x => x.JobDescriptionId,
                        principalTable: "JobDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobSkills",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    JobSkillType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkills", x => new { x.JobId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_JobSkills_JobDescriptions_JobId",
                        column: x => x.JobId,
                        principalTable: "JobDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionJobs",
                columns: table => new
                {
                    ReqId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobDescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionJobs", x => new { x.ReqId, x.JobDescriptionId });
                    table.ForeignKey(
                        name: "FK_RequisitionJobs_JobDescriptions_JobDescriptionId",
                        column: x => x.JobDescriptionId,
                        principalTable: "JobDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisitionJobs_Requisitions_ReqId",
                        column: x => x.ReqId,
                        principalTable: "Requisitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobDescriptionLocation",
                columns: new[] { "Id", "Location" },
                values: new object[,]
                {
                    { 1, "On-Site" },
                    { 2, "Hybrid" },
                    { 3, "Remote" }
                });

            migrationBuilder.InsertData(
                table: "JobDescriptionStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "Closed" },
                    { 3, "On-Hold" }
                });

            migrationBuilder.InsertData(
                table: "RequisitionCurrency",
                columns: new[] { "Id", "Currency" },
                values: new object[,]
                {
                    { 1, "COP" },
                    { 2, "USD" },
                    { 3, "CAD" },
                    { 4, "PHP" },
                    { 5, "EUR" }
                });

            migrationBuilder.InsertData(
                table: "RequisitionHardware",
                columns: new[] { "Id", "Hardware" },
                values: new object[,]
                {
                    { 1, "Dell" },
                    { 2, "Mac" }
                });

            migrationBuilder.InsertData(
                table: "RequisitionInterval",
                columns: new[] { "Id", "Interval" },
                values: new object[,]
                {
                    { 1, "Weekly" },
                    { 2, "Bi-Weekly" },
                    { 3, "Monthly" },
                    { 4, "Bi-Monthly" },
                    { 5, "Quarterly" },
                    { 6, "Bi-Annually" }
                });

            migrationBuilder.InsertData(
                table: "RequisitionLocation",
                columns: new[] { "Id", "Location" },
                values: new object[,]
                {
                    { 1, "Medellín" },
                    { 2, "Bogotá" },
                    { 3, "Cali" },
                    { 4, "Barranquilla" },
                    { 5, "Cartagena" },
                    { 6, "Bucaramanga" },
                    { 7, "Santa Marta" }
                });

            migrationBuilder.InsertData(
                table: "RequisitionStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Cancelled" },
                    { 2, "Closed" },
                    { 3, "Completed" },
                    { 4, "Open" }
                });

            migrationBuilder.InsertData(
                table: "RequisitionWorkType",
                columns: new[] { "Id", "WorkType" },
                values: new object[,]
                {
                    { 1, "Contractor" },
                    { 2, "FullTime" },
                    { 3, "Internship" },
                    { 4, "PartTime" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FirstName", "LastName", "UpdatedAt", "UpdatedBy" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "John@gmail.com", "John", "Smith", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System" });

            migrationBuilder.CreateIndex(
                name: "IX_JobCandidates_CandidateId",
                table: "JobCandidates",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_ApprovalDate",
                table: "JobDescriptions",
                column: "ApprovalDate");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_CreatedAt",
                table: "JobDescriptions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_CreatedBy",
                table: "JobDescriptions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_LocationId",
                table: "JobDescriptions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_StatusId",
                table: "JobDescriptions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_StatusId_CreatedAt",
                table: "JobDescriptions",
                columns: new[] { "StatusId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_Title",
                table: "JobDescriptions",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_SkillId",
                table: "JobSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionJobs_JobDescriptionId",
                table: "RequisitionJobs",
                column: "JobDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_CompensationBandCurrencyId",
                table: "Requisitions",
                column: "CompensationBandCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_CompensationBandIntervalId",
                table: "Requisitions",
                column: "CompensationBandIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_CreatedAt",
                table: "Requisitions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_CreatedBy",
                table: "Requisitions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_HardwareId",
                table: "Requisitions",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_HiringManager",
                table: "Requisitions",
                column: "HiringManager");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_LocationId",
                table: "Requisitions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_StatusId",
                table: "Requisitions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_StatusId_CreatedAt",
                table: "Requisitions",
                columns: new[] { "StatusId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_WorkTypeId",
                table: "Requisitions",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobCandidates");

            migrationBuilder.DropTable(
                name: "JobSkills");

            migrationBuilder.DropTable(
                name: "RequisitionJobs");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "JobDescriptions");

            migrationBuilder.DropTable(
                name: "Requisitions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "JobDescriptionLocation");

            migrationBuilder.DropTable(
                name: "JobDescriptionStatus");

            migrationBuilder.DropTable(
                name: "RequisitionCurrency");

            migrationBuilder.DropTable(
                name: "RequisitionHardware");

            migrationBuilder.DropTable(
                name: "RequisitionInterval");

            migrationBuilder.DropTable(
                name: "RequisitionLocation");

            migrationBuilder.DropTable(
                name: "RequisitionStatus");

            migrationBuilder.DropTable(
                name: "RequisitionWorkType");
        }
    }
}
