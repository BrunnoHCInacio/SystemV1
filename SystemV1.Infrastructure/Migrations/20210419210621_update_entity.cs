using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemV1.Infrastructure.Migrations
{
    public partial class update_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "State",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "State",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "State",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "State",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "Provider",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "Provider",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "Provider",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "Provider",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "ProductItem",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "ProductItem",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "ProductItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "ProductItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "Product",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "Product",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "Country",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "Country",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "Country",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "Country",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "Contact",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "Contact",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "Contact",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "Contact",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "Client",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "Client",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "Client",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "Client",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateChange",
                table: "Address",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "Address",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdUserChange",
                table: "Address",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUserRegister",
                table: "Address",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "State");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "State");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "State");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "State");

            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "ProductItem");

            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "DateChange",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "IdUserChange",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "IdUserRegister",
                table: "Address");
        }
    }
}
