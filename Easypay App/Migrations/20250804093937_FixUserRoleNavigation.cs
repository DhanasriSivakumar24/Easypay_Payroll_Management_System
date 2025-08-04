using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easypay_App.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRoleNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRoleMasterId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserRoleMasterId",
                table: "Employees",
                column: "UserRoleMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_UserRoleMasters_UserRoleMasterId",
                table: "Employees",
                column: "UserRoleMasterId",
                principalTable: "UserRoleMasters",
                principalColumn: "Id");
        }
    }
}
