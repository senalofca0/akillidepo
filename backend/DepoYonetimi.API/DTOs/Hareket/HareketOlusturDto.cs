using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Hareket;

public class HareketOlusturDto
{
    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
    public int UrunId { get; set; }

    [Required(ErrorMessage = "Depo seçimi zorunludur.")]
    public int DepoId { get; set; }

    /// <summary>Giris veya Cikis</summary>
    [Required(ErrorMessage = "Hareket tipi zorunludur.")]
    [RegularExpression("^(Giris|Cikis)$", ErrorMessage = "Hareket tipi 'Giris' veya 'Cikis' olmalıdır.")]
    public string HareketTipi { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Miktar en az 1 olmalıdır.")]
    public int Miktar { get; set; }

    public DateTime? HareketTarihi { get; set; }
    public string? Aciklama { get; set; }

    [MaxLength(50)]
    public string? ReferansNo { get; set; }

    [MaxLength(100)]
    public string IslemYapan { get; set; } = "Sistem";
}
