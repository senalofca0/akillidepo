using DepoYonetimi.API.Data;
using DepoYonetimi.API.Entities;
using DepoYonetimi.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DepoYonetimi.API.Repositories;

public class DepoRepository : IDepoRepository
{
    private readonly DepoDbContext _context;

    public DepoRepository(DepoDbContext context)
    {
        _context = context;
    }

    public async Task<Depo?> GetByIdAsync(int id, string companyId)
    {
        return await _context.Depolar
            .Where(d => d.Id == id && d.CompanyId == companyId && !d.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<(List<Depo> Liste, int ToplamSayisi)> ListeGetirAsync(
        string companyId, string? arama, int sayfa, int sayfaBoyutu)
    {
        var query = _context.Depolar
            .Where(d => d.CompanyId == companyId && !d.IsDeleted);

        if (!string.IsNullOrWhiteSpace(arama))
        {
            query = query.Where(d =>
                d.DepoKodu.Contains(arama) ||
                d.DepoAdi.Contains(arama) ||
                d.Bolge.Contains(arama));
        }

        var toplamSayisi = await query.CountAsync();

        var liste = await query
            .OrderByDescending(d => d.OlusturmaTarihi)
            .Skip((sayfa - 1) * sayfaBoyutu)
            .Take(sayfaBoyutu)
            .ToListAsync();

        return (liste, toplamSayisi);
    }

    public async Task<List<Depo>> HepsiniGetirAsync(string companyId)
    {
        return await _context.Depolar
            .Where(d => d.CompanyId == companyId && d.AktifMi)
            .OrderBy(d => d.DepoAdi)
            .ToListAsync();
    }

    public async Task<bool> KodVarMiAsync(string depoKodu, string companyId, int? haricId = null)
    {
        var query = _context.Depolar
            .Where(d => d.CompanyId == companyId && d.DepoKodu == depoKodu);

        if (haricId.HasValue)
        {
            query = query.Where(d => d.Id != haricId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<Depo> EkleAsync(Depo depo)
    {
        _context.Depolar.Add(depo);
        await _context.SaveChangesAsync();
        return depo;
    }

    public async Task GuncelleAsync(Depo depo)
    {
        depo.GuncellemeTarihi = DateTime.UtcNow;
        _context.Entry(depo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
