using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoruCevapPortali.Migrations
{
    /// <inheritdoc />
    public partial class AddReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReporterId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Reports",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "ReporterId",
                table: "Reports",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IsResolved",
                table: "Reports",
                newName: "is_resolved");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reports",
                newName: "creation_date");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                newName: "IX_Reports_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "Reports",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "is_resolved",
                table: "Reports",
                newName: "IsResolved");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "Reports",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reports",
                newName: "ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                newName: "IX_Reports_ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReporterId",
                table: "Reports",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
