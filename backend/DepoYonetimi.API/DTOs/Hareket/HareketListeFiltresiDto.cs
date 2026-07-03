using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Hareket;

public class HareketListeFiltresiDto
{
    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;

    public int? UrunId { get; set; }
    public int? DepoId { get; set; }
    public string? HareketTipi { get; set; } // Giris, Cikis
    public DateTime? BaslangicTarihi { get; set; }
    public DateTime? BitisTarihi { get; set; }
    public int Sayfa { get; set; } = 1;
    public int SayfaBoyutu { get; set; } = 25;
}
