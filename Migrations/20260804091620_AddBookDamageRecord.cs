using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBookDamageRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DamagedCopies",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookDamageRecords",
                columns: table => new
                {
                    DamageRecordId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BorrowId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BookId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReturnVerificationOfficerId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DamageLevel = table.Column<int>(type: "int", nullable: false),
                    Recommendation = table.Column<int>(type: "int", nullable: false),
                    DamageDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FineAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FineCollected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DamageStatus = table.Column<int>(type: "int", nullable: false),
                    AdminRemarks = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReportedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RepairCompletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DisposedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDamageRecords", x => x.DamageRecordId);
                    table.ForeignKey(
                        name: "FK_BookDamageRecords_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookDamageRecords_BorrowBooks_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "BorrowBooks",
                        principalColumn: "BorrowId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookDamageRecords_LibraryEmployees_ReturnVerificationOfficer~",
                        column: x => x.ReturnVerificationOfficerId,
                        principalTable: "LibraryEmployees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookDamageRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BookDamageRecords_BookId",
                table: "BookDamageRecords",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookDamageRecords_BorrowId",
                table: "BookDamageRecords",
                column: "BorrowId");

            migrationBuilder.CreateIndex(
                name: "IX_BookDamageRecords_ReturnVerificationOfficerId",
                table: "BookDamageRecords",
                column: "ReturnVerificationOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookDamageRecords_UserId",
                table: "BookDamageRecords",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookDamageRecords");

            migrationBuilder.DropColumn(
                name: "DamagedCopies",
                table: "Books");
        }
    }
}
