using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L1.Migrations
{
    public partial class Addeddescriptiontoroomsremovedbookingsmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedFrom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BookedTo",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookedFrom",
                table: "Rooms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookedTo",
                table: "Rooms",
                type: "datetime2",
                nullable: true);
        }
    }
}
