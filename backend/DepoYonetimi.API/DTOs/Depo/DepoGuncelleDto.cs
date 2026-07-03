using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Depo;

public class DepoGuncelleDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Depo adı zorunludur.")]
    [MaxLength(200)]
    public string DepoAdi { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Konum { get; set; }

    [Required]
    [MaxLength(50)]
    public string Bolge { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Raf { get; set; } = string.Empty;

    public int Kapasite { get; set; }
    public bool AktifMi { get; set; }
}
