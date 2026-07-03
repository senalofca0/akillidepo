using System.ComponentModel.DataAnnotations;

namespace DepoYonetimi.API.DTOs.Depo;

public class DepoSilDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "CompanyId zorunludur.")]
    public string CompanyId { get; set; } = string.Empty;
}
