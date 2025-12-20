using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoruCevapPortali.Migrations
{
    /// <inheritdoc />
    public partial class Faz0_Guncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerId1",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId1",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AnswerId1",
                table: "Reports",
                column: "AnswerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_QuestionId1",
                table: "Reports",
                column: "QuestionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Answers_AnswerId1",
                table: "Reports",
                column: "AnswerId1",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Questions_QuestionId1",
                table: "Reports",
                column: "QuestionId1",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Answers_AnswerId1",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Questions_QuestionId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_AnswerId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_QuestionId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AnswerId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "QuestionId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Answers");
        }
    }
}
