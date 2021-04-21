using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemV1.Infrastructure.Migrations
{
    public partial class update_entity_country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "State",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdState",
                table: "Country",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_State_CountryId",
                table: "State",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_State_Country_CountryId",
                table: "State",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_State_Country_CountryId",
                table: "State");

            migrationBuilder.DropIndex(
                name: "IX_State_CountryId",
                table: "State");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "State");

            migrationBuilder.DropColumn(
                name: "IdState",
                table: "Country");
        }
    }
}
