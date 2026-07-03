using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Urun;

public class UrunSilDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;
}
