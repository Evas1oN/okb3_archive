using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace okb3_archive.Migrations
{
    public partial class FileInTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "ArchivedFiles");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "ArchivedFiles",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ZippedFileId",
                table: "ArchivedFiles",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "ZippedFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    File = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZippedFile", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivedFiles_ZippedFileId",
                table: "ArchivedFiles",
                column: "ZippedFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedFiles_ZippedFile_ZippedFileId",
                table: "ArchivedFiles",
                column: "ZippedFileId",
                principalTable: "ZippedFile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedFiles_ZippedFile_ZippedFileId",
                table: "ArchivedFiles");

            migrationBuilder.DropTable(
                name: "ZippedFile");

            migrationBuilder.DropIndex(
                name: "IX_ArchivedFiles_ZippedFileId",
                table: "ArchivedFiles");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "ArchivedFiles");

            migrationBuilder.DropColumn(
                name: "ZippedFileId",
                table: "ArchivedFiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "ArchivedFiles",
                type: "longblob",
                nullable: true);
        }
    }
}
