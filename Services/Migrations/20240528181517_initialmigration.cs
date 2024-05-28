using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentTypes",
                columns: table => new
                {
                    Id_Appoitment_Type = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentTypes", x => x.Id_Appoitment_Type);
                });

            migrationBuilder.CreateTable(
                name: "Clinic_Branches",
                columns: table => new
                {
                    Id_ClinicBranch = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Branch_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinic_Branches", x => x.Id_ClinicBranch);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id_Rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id_Rol);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id_User = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Rol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id_User);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Id_Rol",
                        column: x => x.Id_Rol,
                        principalTable: "Roles",
                        principalColumn: "Id_Rol");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id_Appoitment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_ClinicBranch = table.Column<int>(type: "int", nullable: true),
                    Clinic_BranchId_ClinicBranch = table.Column<int>(type: "int", nullable: true),
                    Id_Appoitment_Type = table.Column<int>(type: "int", nullable: true),
                    AppointmentTypeId_Appoitment_Type = table.Column<int>(type: "int", nullable: true),
                    Id_User = table.Column<int>(type: "int", nullable: true),
                    UserId_User = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id_Appoitment);
                    table.ForeignKey(
                        name: "FK_Appointments_AppointmentTypes_AppointmentTypeId_Appoitment_Type",
                        column: x => x.AppointmentTypeId_Appoitment_Type,
                        principalTable: "AppointmentTypes",
                        principalColumn: "Id_Appoitment_Type",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Clinic_Branches_Clinic_BranchId_ClinicBranch",
                        column: x => x.Clinic_BranchId_ClinicBranch,
                        principalTable: "Clinic_Branches",
                        principalColumn: "Id_ClinicBranch",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_UserId_User",
                        column: x => x.UserId_User,
                        principalTable: "Users",
                        principalColumn: "Id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppointmentTypes",
                columns: new[] { "Id_Appoitment_Type", "Name_type" },
                values: new object[,]
                {
                    { 1, "General Medicine" },
                    { 2, "Dentistry" },
                    { 3, "Pediatrics" },
                    { 4, "Neurology" }
                });

            migrationBuilder.InsertData(
                table: "Clinic_Branches",
                columns: new[] { "Id_ClinicBranch", "Branch_Name" },
                values: new object[,]
                {
                    { 1, "Under Loch Ness" },
                    { 2, "San Martin" },
                    { 3, "Brasilito" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id_Rol", "Name_Rol" },
                values: new object[,]
                {
                    { 1, "USER" },
                    { 2, "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentTypeId_Appoitment_Type",
                table: "Appointments",
                column: "AppointmentTypeId_Appoitment_Type");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Clinic_BranchId_ClinicBranch",
                table: "Appointments",
                column: "Clinic_BranchId_ClinicBranch");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId_User",
                table: "Appointments",
                column: "UserId_User");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id_Rol",
                table: "Users",
                column: "Id_Rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AppointmentTypes");

            migrationBuilder.DropTable(
                name: "Clinic_Branches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
