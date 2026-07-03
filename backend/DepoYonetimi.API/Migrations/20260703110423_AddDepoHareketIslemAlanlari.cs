using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepoYonetimi.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDepoHareketIslemAlanlari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AgirlikKg",
                table: "Urunler",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Urunler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Urunler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuncelleyenKisi",
                table: "Urunler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Urunler",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OlusturanKisi",
                table: "Urunler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RafOmruGun",
                table: "Urunler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResimUrl",
                table: "Urunler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Urunler",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TedarikciKodu",
                table: "Urunler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Urunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Stoklar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Stoklar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuncelleyenKisi",
                table: "Stoklar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stoklar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OlusturanKisi",
                table: "Stoklar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Stoklar",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Stoklar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Depolar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Depolar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuncelleyenKisi",
                table: "Depolar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Depolar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OlusturanKisi",
                table: "Depolar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Depolar",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Depolar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DepoHareketleri",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DepoHareketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuncelleyenKisi",
                table: "DepoHareketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAdresi",
                table: "DepoHareketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IptalEdenHareketId",
                table: "DepoHareketleri",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IptalEdildi",
                table: "DepoHareketleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DepoHareketleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IslemNo",
                table: "DepoHareketleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OlusturanKisi",
                table: "DepoHareketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "DepoHareketleri",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "DepoHareketleri",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgirlikKg",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "GuncelleyenKisi",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "OlusturanKisi",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "RafOmruGun",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "ResimUrl",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "TedarikciKodu",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Stoklar");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Stoklar");

            migrationBuilder.DropColumn(
                name: "GuncelleyenKisi",
                table: "Stoklar");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stoklar");

            migrationBuilder.DropColumn(
                name: "OlusturanKisi",
                table: "Stoklar");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Stoklar");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Stoklar");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Depolar");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Depolar");

            migrationBuilder.DropColumn(
                name: "GuncelleyenKisi",
                table: "Depolar");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Depolar");

            migrationBuilder.DropColumn(
                name: "OlusturanKisi",
                table: "Depolar");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Depolar");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Depolar");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "GuncelleyenKisi",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "IpAdresi",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "IptalEdenHareketId",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "IptalEdildi",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "IslemNo",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "OlusturanKisi",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "DepoHareketleri");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "DepoHareketleri");
        }
    }
}
