using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    /// <inheritdoc />
    public partial class Relationwithroleanduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RolId_Rol",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RolId_Rol",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RolId_Rol",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id_Rol",
                table: "Users",
                column: "Id_Rol");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_Id_Rol",
                table: "Users",
                column: "Id_Rol",
                principalTable: "Roles",
                principalColumn: "Id_Rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_Id_Rol",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Id_Rol",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RolId_Rol",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolId_Rol",
                table: "Users",
                column: "RolId_Rol");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RolId_Rol",
                table: "Users",
                column: "RolId_Rol",
                principalTable: "Roles",
                principalColumn: "Id_Rol");
        }
    }
}
