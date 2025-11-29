using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Tenants");
            
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Properties",
                type: "TEXT",
                nullable: true);
            
            migrationBuilder.Sql("UPDATE Properties SET TenantId = NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Properties");

            migrationBuilder.AddColumn<Guid>(
                name: "PropertyId",
                table: "Tenants",
                type: "TEXT",
                nullable: true);
        }
    }
}
