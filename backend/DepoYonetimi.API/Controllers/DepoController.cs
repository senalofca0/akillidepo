using DepoYonetimi.API.DTOs.Depo;
using DepoYonetimi.API.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepoController : ControllerBase
{
    private readonly IDepoManager _manager;

    public DepoController(IDepoManager manager)
    {
        _manager = manager;
    }

    /// <summary>Sayfalanmış depo listesi</summary>
    [HttpGet("listele")]
    public async Task<IActionResult> Listele(
        [FromQuery] string companyId,
        [FromQuery] string? arama = null,
        [FromQuery] int sayfa = 1,
        [FromQuery] int sayfaBoyutu = 25)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var sonuc = await _manager.ListeGetirAsync(companyId, arama, sayfa, sayfaBoyutu);
        return Ok(sonuc);
    }

    /// <summary>Tüm aktif depolar (dropdown için)</summary>
    [HttpGet("hepsi")]
    public async Task<IActionResult> Hepsi([FromQuery] string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var depolar = await _manager.HepsiniGetirAsync(companyId);
        return Ok(new { success = true, data = depolar });
    }

    /// <summary>Tek depo getir</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, [FromQuery] string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
            return BadRequest(new { success = false, message = "CompanyId zorunludur." });

        var depo = await _manager.GetByIdAsync(id, companyId);
        if (depo == null) return NotFound(new { success = false, message = "Depo bulunamadı." });
        if (depo.CompanyId != companyId) return Forbid();

        return Ok(new { success = true, data = depo });
    }

    /// <summary>Yeni depo oluştur</summary>
    [HttpPost("olustur")]
    public async Task<IActionResult> Olustur([FromBody] DepoOlusturDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState });

        try
        {
            var depo = await _manager.OlusturAsync(dto);
            return Ok(new { success = true, data = depo, message = "Depo başarıyla oluşturuldu." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    /// <summary>Depo güncelle</summary>
    [HttpPost("guncelle")]
    public async Task<IActionResult> Guncelle([FromBody] DepoGuncelleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState });

        try
        {
            var existing = await _manager.GetByIdAsync(dto.Id, dto.CompanyId);
            if (existing == null) return NotFound(new { success = false, message = "Depo bulunamadı." });
            if (existing.CompanyId != dto.CompanyId) return Forbid();

            var depo = await _manager.GuncelleAsync(dto);
            return Ok(new { success = true, data = depo, message = "Depo güncellendi." });
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

    /// <summary>Depo sil (soft delete)</summary>
    [HttpPost("sil")]
    public async Task<IActionResult> Sil([FromBody] DepoSilDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState });

        try
        {
            var existing = await _manager.GetByIdAsync(dto.Id, dto.CompanyId);
            if (existing == null) return NotFound(new { success = false, message = "Depo bulunamadı." });
            if (existing.CompanyId != dto.CompanyId) return Forbid();

            await _manager.SilAsync(dto);
            return Ok(new { success = true, message = "Depo silindi." });
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
}
