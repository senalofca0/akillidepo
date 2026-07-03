using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Stok;
using DepoYonetimi.API.Managers.Interfaces;
using DepoYonetimi.API.Repositories.Interfaces;

namespace DepoYonetimi.API.Managers;

public class StokManager : IStokManager
{
    private readonly IStokRepository _stokRepo;

    public StokManager(IStokRepository stokRepo)
    {
        _stokRepo = stokRepo;
    }

    public async Task<PaginatedResponse<StokDto>> ListeGetirAsync(string companyId, int? urunId, int? depoId, int sayfa, int sayfaBoyutu)
    {
        var (liste, toplamSayisi) = await _stokRepo.ListeGetirAsync(companyId, urunId, depoId, sayfa, sayfaBoyutu);

        var dtoListesi = liste.Select(s => new StokDto
        {
            Id = s.Id,
            CompanyId = s.CompanyId,
            UrunId = s.UrunId,
            UrunKodu = s.Urun?.UrunKodu ?? string.Empty,
            UrunAdi = s.Urun?.UrunAdi ?? string.Empty,
            Kategori = s.Urun?.Kategori ?? string.Empty,
            DepoId = s.DepoId,
            DepoKodu = s.Depo?.DepoKodu ?? string.Empty,
            DepoAdi = s.Depo?.DepoAdi ?? string.Empty,
            Bolge = s.Depo?.Bolge ?? string.Empty,
            Raf = s.Depo?.Raf ?? string.Empty,
            Miktar = s.Miktar,
            MinimumStokSeviyesi = s.MinimumStokSeviyesi,
            MaksimumStokSeviyesi = s.MaksimumStokSeviyesi,
            KritikSeviyede = s.Miktar <= s.MinimumStokSeviyesi
        }).ToList();

        return new PaginatedResponse<StokDto>
        {
            Data = dtoListesi,
            TotalCount = toplamSayisi,
            Page = sayfa,
            PageSize = sayfaBoyutu,
            TotalPages = (int)Math.Ceiling((double)toplamSayisi / sayfaBoyutu)
        };
    }

    public async Task<object> OzetGetirAsync(string companyId)
    {
        var toplamUrun = await _stokRepo.ToplamUrunSayisiAsync(companyId);
        var toplamDepo = await _stokRepo.ToplamDepoSayisiAsync(companyId);
        var kritikStok = await _stokRepo.KritikStokSayisiAsync(companyId);
        var toplamHareket = await _stokRepo.ToplamHareketSayisiAsync(companyId);

        return new
        {
            ToplamUrun = toplamUrun,
            ToplamDepo = toplamDepo,
            KritikStokSayisi = kritikStok,
            ToplamHareket = toplamHareket
        };
    }
}
