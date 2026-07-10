using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    
    public partial class AddMembershipDiscountPercentage : Migration
    {
       
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MembershipDiscountPercentage",
                table: "MembershipPlans",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

       
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipDiscountPercentage",
                table: "MembershipPlans");
        }
    }
}
