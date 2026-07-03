using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Urun;

public class UrunOlusturDto
{
    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ürün kodu zorunludur.")]
    [MaxLength(50)]
    public string UrunKodu { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [MaxLength(200)]
    public string UrunAdi { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    [Required(ErrorMessage = "Kategori zorunludur.")]
    [MaxLength(100)]
    public string Kategori { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Birim { get; set; } = "Adet";

    public decimal? BirimFiyat { get; set; }

    [MaxLength(100)]
    public string? Barkod { get; set; }
}
