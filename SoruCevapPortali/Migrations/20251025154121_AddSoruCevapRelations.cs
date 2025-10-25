using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoruCevapPortali.Migrations
{
    /// <inheritdoc />
    public partial class AddSoruCevapRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KullaniciId1",
                table: "Sorular",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KullaniciId1",
                table: "Cevaplar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_KullaniciId1",
                table: "Sorular",
                column: "KullaniciId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cevaplar_KullaniciId1",
                table: "Cevaplar",
                column: "KullaniciId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cevaplar_Kullanicilar_KullaniciId1",
                table: "Cevaplar",
                column: "KullaniciId1",
                principalTable: "Kullanicilar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sorular_Kullanicilar_KullaniciId1",
                table: "Sorular",
                column: "KullaniciId1",
                principalTable: "Kullanicilar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cevaplar_Kullanicilar_KullaniciId1",
                table: "Cevaplar");

            migrationBuilder.DropForeignKey(
                name: "FK_Sorular_Kullanicilar_KullaniciId1",
                table: "Sorular");

            migrationBuilder.DropIndex(
                name: "IX_Sorular_KullaniciId1",
                table: "Sorular");

            migrationBuilder.DropIndex(
                name: "IX_Cevaplar_KullaniciId1",
                table: "Cevaplar");

            migrationBuilder.DropColumn(
                name: "KullaniciId1",
                table: "Sorular");

            migrationBuilder.DropColumn(
                name: "KullaniciId1",
                table: "Cevaplar");
        }
    }
}
