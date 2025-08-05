using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easypay_App.Migrations
{
    /// <inheritdoc />
    public partial class AddContributionFieldsToBenefitEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EmployeeContribution",
                table: "BenefitMasters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployerContribution",
                table: "BenefitMasters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "BenefitMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmployeeContribution", "EmployerContribution" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "BenefitMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EmployeeContribution", "EmployerContribution" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "BenefitMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EmployeeContribution", "EmployerContribution" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "BenefitMasters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EmployeeContribution", "EmployerContribution" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "BenefitMasters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EmployeeContribution", "EmployerContribution" },
                values: new object[] { 0m, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeContribution",
                table: "BenefitMasters");

            migrationBuilder.DropColumn(
                name: "EmployerContribution",
                table: "BenefitMasters");
        }
    }
}
