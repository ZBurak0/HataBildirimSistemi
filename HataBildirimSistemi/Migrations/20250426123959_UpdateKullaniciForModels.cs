using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HataBildirimSistemi.Migrations
{
    public partial class UpdateKullaniciForModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArızaTur",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArızaTur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Birim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Durum",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Durum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Yetki",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yetki", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArızaBildirim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(maxLength: 50, nullable: true),
                    ArizaTurId = table.Column<int>(nullable: true),
                    KullaniciAd = table.Column<string>(maxLength: 50, nullable: true),
                    BirimId = table.Column<int>(nullable: true),
                    Aciklama = table.Column<string>(maxLength: 350, nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime", nullable: true),
                    DosyaYolu = table.Column<string>(maxLength: 500, nullable: true),
                    DurumId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArızaBildirim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArızaBildirim_ArızaTur",
                        column: x => x.ArizaTurId,
                        principalTable: "ArızaTur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArızaBildirim_Birim",
                        column: x => x.BirimId,
                        principalTable: "Birim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArızaBildirim_Durum_DurumId",
                        column: x => x.DurumId,
                        principalTable: "Durum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(maxLength: 50, nullable: true),
                    Soyad = table.Column<string>(maxLength: 50, nullable: true),
                    TelNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 11, nullable: true),
                    BirimId = table.Column<int>(nullable: true),
                    AKullaniciAd = table.Column<string>(maxLength: 30, nullable: true),
                    ASifre = table.Column<string>(maxLength: 500, nullable: true),
                    YetkiId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admin_Birim",
                        column: x => x.BirimId,
                        principalTable: "Birim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admin_Yetki",
                        column: x => x.YetkiId,
                        principalTable: "Yetki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kullanici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(maxLength: 50, nullable: true),
                    Soyad = table.Column<string>(maxLength: 50, nullable: true),
                    BirimId = table.Column<int>(nullable: true),
                    TelNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 11, nullable: true),
                    KKullaniciAd = table.Column<string>(maxLength: 30, nullable: true),
                    KSifre = table.Column<string>(maxLength: 500, nullable: true),
                    YetkiId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kullanici_Birim",
                        column: x => x.BirimId,
                        principalTable: "Birim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kullanici_Yetki",
                        column: x => x.YetkiId,
                        principalTable: "Yetki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YetkiliServis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(maxLength: 50, nullable: true),
                    Soyad = table.Column<string>(maxLength: 50, nullable: true),
                    TelNo = table.Column<string>(unicode: false, fixedLength: true, maxLength: 11, nullable: true),
                    Mail = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    YKullaniciAd = table.Column<string>(maxLength: 30, nullable: true),
                    YSifre = table.Column<string>(maxLength: 500, nullable: true),
                    YetkiId = table.Column<int>(nullable: true),
                    ArizaTurId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YetkiliServis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YetkiliServis_ArızaTur",
                        column: x => x.ArizaTurId,
                        principalTable: "ArızaTur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_YetkiliServis_Yetki",
                        column: x => x.YetkiId,
                        principalTable: "Yetki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_BirimId",
                table: "Admin",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_YetkiId",
                table: "Admin",
                column: "YetkiId");

            migrationBuilder.CreateIndex(
                name: "IX_ArızaBildirim_ArizaTurId",
                table: "ArızaBildirim",
                column: "ArizaTurId");

            migrationBuilder.CreateIndex(
                name: "IX_ArızaBildirim_BirimId",
                table: "ArızaBildirim",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_ArızaBildirim_DurumId",
                table: "ArızaBildirim",
                column: "DurumId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_BirimId",
                table: "Kullanici",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanici_YetkiId",
                table: "Kullanici",
                column: "YetkiId");

            migrationBuilder.CreateIndex(
                name: "IX_YetkiliServis_ArizaTurId",
                table: "YetkiliServis",
                column: "ArizaTurId");

            migrationBuilder.CreateIndex(
                name: "IX_YetkiliServis_YetkiId",
                table: "YetkiliServis",
                column: "YetkiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "ArızaBildirim");

            migrationBuilder.DropTable(
                name: "Kullanici");

            migrationBuilder.DropTable(
                name: "YetkiliServis");

            migrationBuilder.DropTable(
                name: "Durum");

            migrationBuilder.DropTable(
                name: "Birim");

            migrationBuilder.DropTable(
                name: "ArızaTur");

            migrationBuilder.DropTable(
                name: "Yetki");
        }
    }
}
