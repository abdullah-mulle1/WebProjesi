using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berber.Migrations
{
    public partial class IlkOlusturma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clsnlr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uzmnlk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mst = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clsnlr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hzmtrlr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ucrt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SrDk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hzmtrlr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rndvrlr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClsnId = table.Column<int>(type: "int", nullable: false),
                    HzmtrId = table.Column<int>(type: "int", nullable: false),
                    RndvSt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rndvrlr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rndvrlr_Clsnlr_ClsnId",
                        column: x => x.ClsnId,
                        principalTable: "Clsnlr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rndvrlr_Hzmtrlr_HzmtrId",
                        column: x => x.HzmtrId,
                        principalTable: "Hzmtrlr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rndvrlr_ClsnId",
                table: "Rndvrlr",
                column: "ClsnId");

            migrationBuilder.CreateIndex(
                name: "IX_Rndvrlr_HzmtrId",
                table: "Rndvrlr",
                column: "HzmtrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rndvrlr");

            migrationBuilder.DropTable(
                name: "Clsnlr");

            migrationBuilder.DropTable(
                name: "Hzmtrlr");
        }
    }
}
