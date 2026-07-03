using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Urun;

public class UrunListeFiltresiDto
{
    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;

    public string? Arama { get; set; } // UrunKodu veya UrunAdi
    public string? Kategori { get; set; }
    public int Sayfa { get; set; } = 1;
    public int SayfaBoyutu { get; set; } = 25;
}
