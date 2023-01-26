using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionWeb.Data.Migrations
{
    public partial class ModifiedAspirants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aspirant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyId = table.Column<long>(type: "bigint", nullable: false),
                    ElectionId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aspirant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aspirant_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Aspirant_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Aspirant_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aspirant_ElectionId",
                table: "Aspirant",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Aspirant_PartyId",
                table: "Aspirant",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Aspirant_UserId",
                table: "Aspirant",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aspirant");
        }
    }
}
