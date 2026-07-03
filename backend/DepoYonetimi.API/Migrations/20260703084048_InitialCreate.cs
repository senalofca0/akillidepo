using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepoYonetimi.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepoKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepoAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Konum = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Bolge = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Raf = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kapasite = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depolar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UrunAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kategori = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Birim = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Barkod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepoHareketleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    DepoId = table.Column<int>(type: "int", nullable: false),
                    HareketTipi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Miktar = table.Column<int>(type: "int", nullable: false),
                    HareketTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferansNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IslemYapan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepoHareketleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepoHareketleri_Depolar_DepoId",
                        column: x => x.DepoId,
                        principalTable: "Depolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepoHareketleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stoklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    DepoId = table.Column<int>(type: "int", nullable: false),
                    Miktar = table.Column<int>(type: "int", nullable: false),
                    MinimumStokSeviyesi = table.Column<int>(type: "int", nullable: false),
                    MaksimumStokSeviyesi = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stoklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stoklar_Depolar_DepoId",
                        column: x => x.DepoId,
                        principalTable: "Depolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stoklar_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepoHareketleri_CompanyId",
                table: "DepoHareketleri",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DepoHareketleri_DepoId",
                table: "DepoHareketleri",
                column: "DepoId");

            migrationBuilder.CreateIndex(
                name: "IX_DepoHareketleri_HareketTarihi",
                table: "DepoHareketleri",
                column: "HareketTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_DepoHareketleri_UrunId",
                table: "DepoHareketleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_Depolar_CompanyId",
                table: "Depolar",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Depolar_CompanyId_DepoKodu",
                table: "Depolar",
                columns: new[] { "CompanyId", "DepoKodu" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_CompanyId",
                table: "Stoklar",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_CompanyId_UrunId_DepoId",
                table: "Stoklar",
                columns: new[] { "CompanyId", "UrunId", "DepoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_DepoId",
                table: "Stoklar",
                column: "DepoId");

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_UrunId",
                table: "Stoklar",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_CompanyId",
                table: "Urunler",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_CompanyId_UrunKodu",
                table: "Urunler",
                columns: new[] { "CompanyId", "UrunKodu" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepoHareketleri");

            migrationBuilder.DropTable(
                name: "Stoklar");

            migrationBuilder.DropTable(
                name: "Depolar");

            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
