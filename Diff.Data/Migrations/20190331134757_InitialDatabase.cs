using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diff.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiffAnalysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Left = table.Column<byte[]>(nullable: true),
                    Right = table.Column<byte[]>(nullable: true),
                    Analized = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiffAnalysis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiffSegment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Offset = table.Column<int>(nullable: false),
                    Length = table.Column<int>(nullable: false),
                    DiffAnalysisId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiffSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiffSegment_DiffAnalysis_DiffAnalysisId",
                        column: x => x.DiffAnalysisId,
                        principalTable: "DiffAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiffSegment_DiffAnalysisId",
                table: "DiffSegment",
                column: "DiffAnalysisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiffSegment");

            migrationBuilder.DropTable(
                name: "DiffAnalysis");
        }
    }
}
