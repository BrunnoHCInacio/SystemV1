using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemV1.Infrastructure.Migrations
{
    public partial class update_country_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdState",
                table: "Country");

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "State",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateId",
                table: "State");

            migrationBuilder.AddColumn<Guid>(
                name: "IdState",
                table: "Country",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
