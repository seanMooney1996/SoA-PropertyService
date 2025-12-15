using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Addauthrelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthenticationId",
                table: "Tenants",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthenticationId",
                table: "Landlords",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_AuthenticationId",
                table: "Tenants",
                column: "AuthenticationId");

            migrationBuilder.CreateIndex(
                name: "IX_Landlords_AuthenticationId",
                table: "Landlords",
                column: "AuthenticationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Landlords_Authentications_AuthenticationId",
                table: "Landlords",
                column: "AuthenticationId",
                principalTable: "Authentications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Authentications_AuthenticationId",
                table: "Tenants",
                column: "AuthenticationId",
                principalTable: "Authentications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Landlords_Authentications_AuthenticationId",
                table: "Landlords");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Authentications_AuthenticationId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_AuthenticationId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Landlords_AuthenticationId",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "AuthenticationId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "AuthenticationId",
                table: "Landlords");
        }
    }
}
