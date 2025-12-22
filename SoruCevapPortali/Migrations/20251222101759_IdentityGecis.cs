using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoruCevapPortali.Migrations
{
    /// <inheritdoc />
    public partial class IdentityGecis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- ESKİ HATALI BAĞLANTILARI KALDIRMA (TEMİZLİK) ---
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerLikes_Users_UserId",
                table: "AnswerLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_UserId",
                table: "Answers");

            // Eğer veritabanında varsa hatalı olanı düşür
            try
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_Answers_Users_UserId1",
                    table: "Answers");
            }
            catch { }

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_UserId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_UserId",
                table: "Questions");

            // Eğer veritabanında varsa hatalı olanı düşür
            try
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_Questions_Users_UserId1",
                    table: "Questions");
            }
            catch { }

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Answers_AnswerId",
                table: "Reports");

            // Eğer veritabanında varsa hatalı olanı düşür
            try
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_Reports_Answers_AnswerId1",
                    table: "Reports");
            }
            catch { }

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Questions_QuestionId",
                table: "Reports");

            // Eğer veritabanında varsa hatalı olanı düşür
            try
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_Reports_Questions_QuestionId1",
                    table: "Reports");
            }
            catch { }

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports");

            // --- ESKİ HATALI INDEXLERİ SİLME ---
            // Try-Catch içine aldım ki yoksa bile hata verip durmasın
            try { migrationBuilder.DropIndex(name: "IX_Reports_AnswerId1", table: "Reports"); } catch { }
            try { migrationBuilder.DropIndex(name: "IX_Reports_QuestionId1", table: "Reports"); } catch { }
            try { migrationBuilder.DropIndex(name: "IX_Questions_UserId1", table: "Questions"); } catch { }
            try { migrationBuilder.DropIndex(name: "IX_Answers_UserId1", table: "Answers"); } catch { }

            // --- ANAHTARLARI DÜZENLEME ---
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            // --- HATALI KOLONLARI SİLME (Bunlar veritabanında fazlalıktı) ---
            try { migrationBuilder.DropColumn(name: "AnswerId1", table: "Reports"); } catch { }
            try { migrationBuilder.DropColumn(name: "QuestionId1", table: "Reports"); } catch { }
            try { migrationBuilder.DropColumn(name: "UserId1", table: "Questions"); } catch { }
            try { migrationBuilder.DropColumn(name: "UserId1", table: "Answers"); } catch { }

            // --- TABLO ADINI GÜNCELLEME (Identity Standardı) ---
            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AspNetUsers");

            // --- KOLON TİPLERİNİ GÜNCELLEME ---
            migrationBuilder.AlterColumn<string>(
                name: "User_name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // --- IDENTITY KOLONLARINI EKLEME ---
            migrationBuilder.AddColumn<int>(name: "AccessFailedCount", table: "AspNetUsers", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<string>(name: "ConcurrencyStamp", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<bool>(name: "EmailConfirmed", table: "AspNetUsers", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "LockoutEnabled", table: "AspNetUsers", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<DateTimeOffset>(name: "LockoutEnd", table: "AspNetUsers", type: "datetimeoffset", nullable: true);
            migrationBuilder.AddColumn<string>(name: "NormalizedEmail", table: "AspNetUsers", type: "nvarchar(256)", maxLength: 256, nullable: true);
            migrationBuilder.AddColumn<string>(name: "NormalizedUserName", table: "AspNetUsers", type: "nvarchar(256)", maxLength: 256, nullable: true);
            migrationBuilder.AddColumn<string>(name: "PasswordHash", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "PhoneNumber", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<bool>(name: "PhoneNumberConfirmed", table: "AspNetUsers", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<string>(name: "SecurityStamp", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<bool>(name: "TwoFactorEnabled", table: "AspNetUsers", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<string>(name: "UserName", table: "AspNetUsers", type: "nvarchar(256)", maxLength: 256, nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            // --- IDENTITY TABLOLARINI OLUŞTURMA ---
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "EmailIndex", table: "AspNetUsers", column: "NormalizedEmail");
            migrationBuilder.CreateIndex(name: "UserNameIndex", table: "AspNetUsers", column: "NormalizedUserName", unique: true, filter: "[NormalizedUserName] IS NOT NULL");
            migrationBuilder.CreateIndex(name: "IX_AspNetRoleClaims_RoleId", table: "AspNetRoleClaims", column: "RoleId");
            migrationBuilder.CreateIndex(name: "RoleNameIndex", table: "AspNetRoles", column: "NormalizedName", unique: true, filter: "[NormalizedName] IS NOT NULL");
            migrationBuilder.CreateIndex(name: "IX_AspNetUserClaims_UserId", table: "AspNetUserClaims", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_AspNetUserLogins_UserId", table: "AspNetUserLogins", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_AspNetUserRoles_RoleId", table: "AspNetUserRoles", column: "RoleId");

            // --- YENİ BAĞLANTILAR (UserId kullananlar - UserId1 YOK!) ---

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerLikes_AspNetUsers_UserId",
                table: "AnswerLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_UserId",
                table: "Answers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Answers_AnswerId",
                table: "Reports",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_UserId",
                table: "Reports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Questions_QuestionId",
                table: "Reports",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Down metodu geri alma içindir, şu an önemli değil ama boş bıraktım hata vermesin.
        }
    }
}