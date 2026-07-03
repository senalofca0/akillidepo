using DepoYonetimi.API.DTOs.Hareket;
using DepoYonetimi.API.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HareketController : ControllerBase
{
    private readonly IHareketManager _manager;

    public HareketController(IHareketManager manager)
    {
        _manager = manager;
    }

    /// <summary>Sayfalanmış hareket listesi (filtre destekli)</summary>
    [HttpGet("listele")]
    public async Task<IActionResult> Listele([FromQuery] HareketListeFiltresiDto filtre)
    {
        if (string.IsNullOrWhiteSpace(filtre.CompanyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var sonuc = await _manager.ListeGetirAsync(filtre);
        return Ok(sonuc);
    }

    /// <summary>Depoya ürün girişi veya depodан çıkışı yap</summary>
    [HttpPost("isle")]
    public async Task<IActionResult> Isle([FromBody] HareketOlusturDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState });

        if (string.IsNullOrWhiteSpace(dto.CompanyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        try
        {
            var hareket = await _manager.OlusturAsync(dto);
            var mesaj = dto.HareketTipi == "Giris"
                ? "Ürün depoya başarıyla girildi."
                : "Ürün depodan başarıyla çıkarıldı.";
            return Ok(new { success = true, data = hareket, message = mesaj });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}
