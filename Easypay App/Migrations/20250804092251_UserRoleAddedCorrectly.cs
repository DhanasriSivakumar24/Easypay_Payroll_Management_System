using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easypay_App.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleAddedCorrectly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRoleId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserRoleMasterId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserRoleId",
                table: "Employees",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserRoleMasterId",
                table: "Employees",
                column: "UserRoleMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_UserRole",
                table: "Employees",
                column: "UserRoleId",
                principalTable: "UserRoleMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_UserRoleMasters_UserRoleMasterId",
                table: "Employees",
                column: "UserRoleMasterId",
                principalTable: "UserRoleMasters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_UserRole",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_UserRoleMasters_UserRoleMasterId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserRoleId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserRoleMasterId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserRoleMasterId",
                table: "Employees");
        }
    }
}
