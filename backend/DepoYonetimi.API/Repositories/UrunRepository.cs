using DepoYonetimi.API.Data;
using DepoYonetimi.API.Entities;
using DepoYonetimi.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DepoYonetimi.API.Repositories;

public class UrunRepository : IUrunRepository
{
    private readonly DepoDbContext _context;

    public UrunRepository(DepoDbContext context)
    {
        _context = context;
    }

    public async Task<Urun?> GetByIdAsync(int id, string companyId)
    {
        return await _context.Urunler
            .Where(u => u.Id == id && u.CompanyId == companyId && !u.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<(List<Urun> Liste, int ToplamSayisi)> ListeGetirAsync(
        string companyId, string? arama, string? kategori, int sayfa, int sayfaBoyutu)
    {
        var query = _context.Urunler
            .Where(u => u.CompanyId == companyId && !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(arama))
        {
            query = query.Where(u =>
                u.UrunKodu.Contains(arama) ||
                u.UrunAdi.Contains(arama) ||
                (u.Barkod != null && u.Barkod.Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(kategori))
        {
            query = query.Where(u => u.Kategori == kategori);
        }

        var toplamSayisi = await query.CountAsync();

        var liste = await query
            .OrderByDescending(u => u.OlusturmaTarihi)
            .Skip((sayfa - 1) * sayfaBoyutu)
            .Take(sayfaBoyutu)
            .ToListAsync();

        return (liste, toplamSayisi);
    }

    public async Task<bool> KodVarMiAsync(string urunKodu, string companyId, int? haricId = null)
    {
        var query = _context.Urunler
            .Where(u => u.CompanyId == companyId && u.UrunKodu == urunKodu);

        if (haricId.HasValue)
        {
            query = query.Where(u => u.Id != haricId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<Urun> EkleAsync(Urun urun)
    {
        _context.Urunler.Add(urun);
        await _context.SaveChangesAsync();
        return urun;
    }

    public async Task GuncelleAsync(Urun urun)
    {
        urun.GuncellemeTarihi = DateTime.UtcNow;
        _context.Entry(urun).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> KategorileriGetirAsync(string companyId)
    {
        return await _context.Urunler
            .Where(u => u.CompanyId == companyId)
            .Select(u => u.Kategori)
            .Distinct()
            .OrderBy(k => k)
            .ToListAsync();
    }
}
