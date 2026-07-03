using DepoYonetimi.API.DTOs.Urun;
using DepoYonetimi.API.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrunController : ControllerBase
{
    private readonly IUrunManager _manager;

    public UrunController(IUrunManager manager)
    {
        _manager = manager;
    }

    /// <summary>Sayfalanmış ürün listesi (arama ve filtre destekli)</summary>
    [HttpGet("listele")]
    public async Task<IActionResult> Listele([FromQuery] UrunListeFiltresiDto filtre)
    {
        if (string.IsNullOrWhiteSpace(filtre.CompanyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var sonuc = await _manager.ListeGetirAsync(filtre);
        return Ok(sonuc);
    }

    /// <summary>Tek ürün getir</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, [FromQuery] string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var urun = await _manager.GetByIdAsync(id, companyId);
        if (urun == null) return NotFound(new { success = false, message = "Ürün bulunamadı." });
        if (urun.CompanyId != companyId) return Forbid();

        return Ok(new { success = true, data = urun });
    }

    /// <summary>Yeni ürün oluştur</summary>
    [HttpPost("olustur")]
    public async Task<IActionResult> Olustur([FromBody] UrunOlusturDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState });

        try
        {
            var urun = await _manager.OlusturAsync(dto);
            return Ok(new { success = true, data = urun, message = "Ürün başarıyla oluşturuldu." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    /// <summary>Ürün güncelle</summary>
    [HttpPost("guncelle")]
    public async Task<IActionResult> Guncelle([FromBody] UrunGuncelleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState });

        try
        {
            var existing = await _manager.GetByIdAsync(dto.Id, dto.CompanyId);
            if (existing == null) return NotFound(new { success = false, message = "Ürün bulunamadı." });
            if (existing.CompanyId != dto.CompanyId) return Forbid();

            var urun = await _manager.GuncelleAsync(dto);
            return Ok(new { success = true, data = urun, message = "Ürün güncellendi." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    /// <summary>Ürün sil (soft delete)</summary>
    [HttpPost("sil")]
    public async Task<IActionResult> Sil([FromBody] UrunSilDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState });

        try
        {
            var existing = await _manager.GetByIdAsync(dto.Id, dto.CompanyId);
            if (existing == null) return NotFound(new { success = false, message = "Ürün bulunamadı." });
            if (existing.CompanyId != dto.CompanyId) return Forbid();

            await _manager.SilAsync(dto);
            return Ok(new { success = true, message = "Ürün silindi." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    /// <summary>Kategorileri getir (dropdown için)</summary>
    [HttpGet("kategoriler")]
    public async Task<IActionResult> Kategoriler([FromQuery] string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var kategoriler = await _manager.KategorileriGetirAsync(companyId);
        return Ok(new { success = true, data = kategoriler });
    }
}
