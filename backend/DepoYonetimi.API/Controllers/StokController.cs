using DepoYonetimi.API.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StokController : ControllerBase
{
    private readonly IStokManager _manager;

    public StokController(IStokManager manager)
    {
        _manager = manager;
    }

    /// <summary>Sayfalanmış stok listesi</summary>
    [HttpGet("listele")]
    public async Task<IActionResult> Listele(
        [FromQuery] string companyId,
        [FromQuery] int? urunId = null,
        [FromQuery] int? depoId = null,
        [FromQuery] int sayfa = 1,
        [FromQuery] int sayfaBoyutu = 25)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var sonuc = await _manager.ListeGetirAsync(companyId, urunId, depoId, sayfa, sayfaBoyutu);
        return Ok(sonuc);
    }

    /// <summary>Dashboard özet bilgileri</summary>
    [HttpGet("ozet")]
    public async Task<IActionResult> Ozet([FromQuery] string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var ozet = await _manager.OzetGetirAsync(companyId);
        return Ok(new { success = true, data = ozet });
    }
}
