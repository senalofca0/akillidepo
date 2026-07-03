using DepoYonetimi.API.Data;
using DepoYonetimi.API.Entities;
using DepoYonetimi.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DepoYonetimi.API.Repositories;

public class HareketRepository : IHareketRepository
{
    private readonly DepoDbContext _context;

    public HareketRepository(DepoDbContext context)
    {
        _context = context;
    }

    public async Task<(List<DepoHareket> Liste, int ToplamSayisi)> ListeGetirAsync(
        string companyId, int? urunId, int? depoId,
        string? hareketTipi, DateTime? baslangic, DateTime? bitis,
        int sayfa, int sayfaBoyutu)
    {
        var query = _context.DepoHareketleri
            .Include(h => h.Urun)
            .Include(h => h.Depo)
            .Where(h => h.CompanyId == companyId);

        if (urunId.HasValue)
            query = query.Where(h => h.UrunId == urunId.Value);

        if (depoId.HasValue)
            query = query.Where(h => h.DepoId == depoId.Value);

        if (!string.IsNullOrWhiteSpace(hareketTipi))
            query = query.Where(h => h.HareketTipi == hareketTipi);

        if (baslangic.HasValue)
            query = query.Where(h => h.HareketTarihi >= baslangic.Value);

        if (bitis.HasValue)
            query = query.Where(h => h.HareketTarihi <= bitis.Value);

        var toplamSayisi = await query.CountAsync();

        var liste = await query
            .OrderByDescending(h => h.HareketTarihi)
            .Skip((sayfa - 1) * sayfaBoyutu)
            .Take(sayfaBoyutu)
            .ToListAsync();

        return (liste, toplamSayisi);
    }

    public async Task<DepoHareket> EkleAsync(DepoHareket hareket)
    {
        _context.DepoHareketleri.Add(hareket);
        await _context.SaveChangesAsync();
        return hareket;
    }
}
