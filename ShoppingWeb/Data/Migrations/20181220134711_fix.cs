using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingWeb.Data.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ProductsSelectedForAppointments",
                newName: "Quatity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quatity",
                table: "ProductsSelectedForAppointments",
                newName: "Quantity");
        }
    }
}
