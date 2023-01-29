using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionWeb.Data.Migrations
{
    public partial class BaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Wards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "StateRegions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Results",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ReportCase",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "PollingUnits",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "PartyResults",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Parties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "lGAs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ElectionTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Elections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Constituencies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Aspirants",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StateRegions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ReportCase");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PollingUnits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PartyResults");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "lGAs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ElectionTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Constituencies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Aspirants");
        }
    }
}
