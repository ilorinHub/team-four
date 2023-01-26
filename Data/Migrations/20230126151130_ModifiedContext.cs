using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionWeb.Data.Migrations
{
    public partial class ModifiedContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aspirant_AspNetUsers_UserId",
                table: "Aspirant");

            migrationBuilder.DropForeignKey(
                name: "FK_Aspirant_Election_ElectionId",
                table: "Aspirant");

            migrationBuilder.DropForeignKey(
                name: "FK_Aspirant_Party_PartyId",
                table: "Aspirant");

            migrationBuilder.DropForeignKey(
                name: "FK_Election_Countries_CountryId",
                table: "Election");

            migrationBuilder.DropForeignKey(
                name: "FK_Election_ElectionType_ElectionTypeId",
                table: "Election");

            migrationBuilder.DropForeignKey(
                name: "FK_Party_Countries_CountryId",
                table: "Party");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyResults_Party_PartyId",
                table: "PartyResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Election_ElectionId",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Party",
                table: "Party");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectionType",
                table: "ElectionType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Election",
                table: "Election");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aspirant",
                table: "Aspirant");

            migrationBuilder.RenameTable(
                name: "Party",
                newName: "Parties");

            migrationBuilder.RenameTable(
                name: "ElectionType",
                newName: "ElectionTypes");

            migrationBuilder.RenameTable(
                name: "Election",
                newName: "Elections");

            migrationBuilder.RenameTable(
                name: "Aspirant",
                newName: "Aspirants");

            migrationBuilder.RenameIndex(
                name: "IX_Party_CountryId",
                table: "Parties",
                newName: "IX_Parties_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Election_ElectionTypeId",
                table: "Elections",
                newName: "IX_Elections_ElectionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Election_CountryId",
                table: "Elections",
                newName: "IX_Elections_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Aspirant_UserId",
                table: "Aspirants",
                newName: "IX_Aspirants_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Aspirant_PartyId",
                table: "Aspirants",
                newName: "IX_Aspirants_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Aspirant_ElectionId",
                table: "Aspirants",
                newName: "IX_Aspirants_ElectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parties",
                table: "Parties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectionTypes",
                table: "ElectionTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Elections",
                table: "Elections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aspirants",
                table: "Aspirants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aspirants_AspNetUsers_UserId",
                table: "Aspirants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aspirants_Elections_ElectionId",
                table: "Aspirants",
                column: "ElectionId",
                principalTable: "Elections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aspirants_Parties_PartyId",
                table: "Aspirants",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Elections_Countries_CountryId",
                table: "Elections",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Elections_ElectionTypes_ElectionTypeId",
                table: "Elections",
                column: "ElectionTypeId",
                principalTable: "ElectionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Countries_CountryId",
                table: "Parties",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyResults_Parties_PartyId",
                table: "PartyResults",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Elections_ElectionId",
                table: "Results",
                column: "ElectionId",
                principalTable: "Elections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aspirants_AspNetUsers_UserId",
                table: "Aspirants");

            migrationBuilder.DropForeignKey(
                name: "FK_Aspirants_Elections_ElectionId",
                table: "Aspirants");

            migrationBuilder.DropForeignKey(
                name: "FK_Aspirants_Parties_PartyId",
                table: "Aspirants");

            migrationBuilder.DropForeignKey(
                name: "FK_Elections_Countries_CountryId",
                table: "Elections");

            migrationBuilder.DropForeignKey(
                name: "FK_Elections_ElectionTypes_ElectionTypeId",
                table: "Elections");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Countries_CountryId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyResults_Parties_PartyId",
                table: "PartyResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Elections_ElectionId",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parties",
                table: "Parties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectionTypes",
                table: "ElectionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Elections",
                table: "Elections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aspirants",
                table: "Aspirants");

            migrationBuilder.RenameTable(
                name: "Parties",
                newName: "Party");

            migrationBuilder.RenameTable(
                name: "ElectionTypes",
                newName: "ElectionType");

            migrationBuilder.RenameTable(
                name: "Elections",
                newName: "Election");

            migrationBuilder.RenameTable(
                name: "Aspirants",
                newName: "Aspirant");

            migrationBuilder.RenameIndex(
                name: "IX_Parties_CountryId",
                table: "Party",
                newName: "IX_Party_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Elections_ElectionTypeId",
                table: "Election",
                newName: "IX_Election_ElectionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Elections_CountryId",
                table: "Election",
                newName: "IX_Election_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Aspirants_UserId",
                table: "Aspirant",
                newName: "IX_Aspirant_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Aspirants_PartyId",
                table: "Aspirant",
                newName: "IX_Aspirant_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Aspirants_ElectionId",
                table: "Aspirant",
                newName: "IX_Aspirant_ElectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Party",
                table: "Party",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectionType",
                table: "ElectionType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Election",
                table: "Election",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aspirant",
                table: "Aspirant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aspirant_AspNetUsers_UserId",
                table: "Aspirant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aspirant_Election_ElectionId",
                table: "Aspirant",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aspirant_Party_PartyId",
                table: "Aspirant",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Election_Countries_CountryId",
                table: "Election",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Election_ElectionType_ElectionTypeId",
                table: "Election",
                column: "ElectionTypeId",
                principalTable: "ElectionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Party_Countries_CountryId",
                table: "Party",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyResults_Party_PartyId",
                table: "PartyResults",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Election_ElectionId",
                table: "Results",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
