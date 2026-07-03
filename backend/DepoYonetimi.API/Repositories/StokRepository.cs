using DepoYonetimi.API.Data;
using DepoYonetimi.API.Entities;
using DepoYonetimi.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DepoYonetimi.API.Repositories;

public class StokRepository : IStokRepository
{
    private readonly DepoDbContext _context;

    public StokRepository(DepoDbContext context)
    {
        _context = context;
    }

    public async Task<Stok?> GetByUrunDepoAsync(int urunId, int depoId, string companyId)
    {
        return await _context.Stoklar
            .Include(s => s.Urun)
            .Include(s => s.Depo)
            .Where(s => s.UrunId == urunId && s.DepoId == depoId && s.CompanyId == companyId)
            .FirstOrDefaultAsync();
    }

    public async Task<(List<Stok> Liste, int ToplamSayisi)> ListeGetirAsync(
        string companyId, int? urunId, int? depoId, int sayfa, int sayfaBoyutu)
    {
        var query = _context.Stoklar
            .Include(s => s.Urun)
            .Include(s => s.Depo)
            .Where(s => s.CompanyId == companyId);

        if (urunId.HasValue)
        {
            query = query.Where(s => s.UrunId == urunId.Value);
        }

        if (depoId.HasValue)
        {
            query = query.Where(s => s.DepoId == depoId.Value);
        }

        var toplamSayisi = await query.CountAsync();

        var liste = await query
            .OrderByDescending(s => s.OlusturmaTarihi)
            .Skip((sayfa - 1) * sayfaBoyutu)
            .Take(sayfaBoyutu)
            .ToListAsync();

        return (liste, toplamSayisi);
    }

    public async Task<List<Stok>> KritikStokGetirAsync(string companyId)
    {
        return await _context.Stoklar
            .Include(s => s.Urun)
            .Include(s => s.Depo)
            .Where(s => s.CompanyId == companyId && s.Miktar <= s.MinimumStokSeviyesi)
            .OrderBy(s => s.Miktar)
            .ToListAsync();
    }

    public async Task<int> ToplamStokGetirAsync(int urunId, string companyId)
    {
        return await _context.Stoklar
            .Where(s => s.UrunId == urunId && s.CompanyId == companyId)
            .SumAsync(s => s.Miktar);
    }

    public async Task<Stok> EkleAsync(Stok stok)
    {
        _context.Stoklar.Add(stok);
        await _context.SaveChangesAsync();
        return stok;
    }

    public async Task GuncelleAsync(Stok stok)
    {
        stok.GuncellemeTarihi = DateTime.UtcNow;
        _context.Entry(stok).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<int> ToplamUrunSayisiAsync(string companyId)
    {
        return await _context.Urunler
            .Where(u => u.CompanyId == companyId)
            .CountAsync();
    }

    public async Task<int> ToplamDepoSayisiAsync(string companyId)
    {
        return await _context.Depolar
            .Where(d => d.CompanyId == companyId && d.AktifMi)
            .CountAsync();
    }

    public async Task<int> KritikStokSayisiAsync(string companyId)
    {
        return await _context.Stoklar
            .Where(s => s.CompanyId == companyId && s.Miktar <= s.MinimumStokSeviyesi)
            .CountAsync();
    }

    public async Task<int> ToplamHareketSayisiAsync(string companyId)
    {
        return await _context.DepoHareketleri
            .Where(h => h.CompanyId == companyId)
            .CountAsync();
    }
}
