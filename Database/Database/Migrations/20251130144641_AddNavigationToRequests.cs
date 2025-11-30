using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationToRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_PropertyId",
                table: "RentalRequests",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_TenantId",
                table: "RentalRequests",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Properties_PropertyId",
                table: "RentalRequests",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Tenants_TenantId",
                table: "RentalRequests",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Properties_PropertyId",
                table: "RentalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Tenants_TenantId",
                table: "RentalRequests");

            migrationBuilder.DropIndex(
                name: "IX_RentalRequests_PropertyId",
                table: "RentalRequests");

            migrationBuilder.DropIndex(
                name: "IX_RentalRequests_TenantId",
                table: "RentalRequests");
        }
    }
}
