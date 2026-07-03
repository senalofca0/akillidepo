using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Urun;

public class UrunGuncelleDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [MaxLength(200)]
    public string UrunAdi { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    [Required]
    [MaxLength(100)]
    public string Kategori { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Birim { get; set; } = "Adet";

    public decimal? BirimFiyat { get; set; }

    [MaxLength(100)]
    public string? Barkod { get; set; }
}
