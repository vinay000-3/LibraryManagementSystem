using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnWorkflowFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DamageFine",
                table: "BorrowBooks",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "DamageFinePaid",
                table: "BorrowBooks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "LateFine",
                table: "BorrowBooks",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "LateFinePaid",
                table: "BorrowBooks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LibrarianId",
                table: "BorrowBooks",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ReturnVerificationOfficerId",
                table: "BorrowBooks",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowBooks_LibrarianId",
                table: "BorrowBooks",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowBooks_ReturnVerificationOfficerId",
                table: "BorrowBooks",
                column: "ReturnVerificationOfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowBooks_LibraryEmployees_LibrarianId",
                table: "BorrowBooks",
                column: "LibrarianId",
                principalTable: "LibraryEmployees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowBooks_LibraryEmployees_ReturnVerificationOfficerId",
                table: "BorrowBooks",
                column: "ReturnVerificationOfficerId",
                principalTable: "LibraryEmployees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowBooks_LibraryEmployees_LibrarianId",
                table: "BorrowBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowBooks_LibraryEmployees_ReturnVerificationOfficerId",
                table: "BorrowBooks");

            migrationBuilder.DropIndex(
                name: "IX_BorrowBooks_LibrarianId",
                table: "BorrowBooks");

            migrationBuilder.DropIndex(
                name: "IX_BorrowBooks_ReturnVerificationOfficerId",
                table: "BorrowBooks");

            migrationBuilder.DropColumn(
                name: "DamageFine",
                table: "BorrowBooks");

            migrationBuilder.DropColumn(
                name: "DamageFinePaid",
                table: "BorrowBooks");

            migrationBuilder.DropColumn(
                name: "LateFine",
                table: "BorrowBooks");

            migrationBuilder.DropColumn(
                name: "LateFinePaid",
                table: "BorrowBooks");

            migrationBuilder.DropColumn(
                name: "LibrarianId",
                table: "BorrowBooks");

            migrationBuilder.DropColumn(
                name: "ReturnVerificationOfficerId",
                table: "BorrowBooks");
        }
    }
}
