using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Urun;
using DepoYonetimi.API.Entities;
using DepoYonetimi.API.Managers.Interfaces;
using DepoYonetimi.API.Repositories.Interfaces;

namespace DepoYonetimi.API.Managers;

public class UrunManager : IUrunManager
{
    private readonly IUrunRepository _urunRepo;
    private readonly IStokRepository _stokRepo;

    public UrunManager(IUrunRepository urunRepo, IStokRepository stokRepo)
    {
        _urunRepo = urunRepo;
        _stokRepo = stokRepo;
    }

    public async Task<UrunDto?> GetByIdAsync(int id, string companyId)
    {
        var urun = await _urunRepo.GetByIdAsync(id, companyId);
        if (urun == null) return null;

        var toplamStok = await _stokRepo.ToplamStokGetirAsync(id, companyId);
        return UrunToDto(urun, toplamStok);
    }

    public async Task<PaginatedResponse<UrunDto>> ListeGetirAsync(UrunListeFiltresiDto filtre)
    {
        var (liste, toplamSayisi) = await _urunRepo.ListeGetirAsync(
            filtre.CompanyId, filtre.Arama, filtre.Kategori, filtre.Sayfa, filtre.SayfaBoyutu);

        // Tek sorguda tüm ürünlerin stok toplamlarını çek (N+1 önlenir)
        var urunIdleri = liste.Select(u => u.Id).ToList();
        var stokToplami = await _stokRepo.ToplamStokBulkGetirAsync(urunIdleri, filtre.CompanyId);

        var dtoListesi = liste
            .Select(u => UrunToDto(u, stokToplami.GetValueOrDefault(u.Id, 0)))
            .ToList();

        return new PaginatedResponse<UrunDto>
        {
            Data = dtoListesi,
            TotalCount = toplamSayisi,
            Page = filtre.Sayfa,
            PageSize = filtre.SayfaBoyutu,
            TotalPages = (int)Math.Ceiling((double)toplamSayisi / filtre.SayfaBoyutu)
        };
    }

    public async Task<UrunDto> OlusturAsync(UrunOlusturDto dto)
    {
        if (await _urunRepo.KodVarMiAsync(dto.UrunKodu, dto.CompanyId))
            throw new InvalidOperationException($"'{dto.UrunKodu}' ürün kodu zaten kullanımda.");

        var urun = new Urun
        {
            CompanyId = dto.CompanyId,
            UrunKodu = dto.UrunKodu.ToUpperInvariant(),
            UrunAdi = dto.UrunAdi,
            Aciklama = dto.Aciklama,
            Kategori = dto.Kategori,
            Birim = dto.Birim,
            BirimFiyat = dto.BirimFiyat,
            Barkod = dto.Barkod,
            OlusturmaTarihi = DateTime.UtcNow
        };

        var eklenen = await _urunRepo.EkleAsync(urun);
        return UrunToDto(eklenen, 0);
    }

    public async Task<UrunDto> GuncelleAsync(UrunGuncelleDto dto)
    {
        var urun = await _urunRepo.GetByIdAsync(dto.Id, dto.CompanyId)
            ?? throw new KeyNotFoundException("Ürün bulunamadı.");

        if (urun.CompanyId != dto.CompanyId)
            throw new UnauthorizedAccessException("Bu ürüne erişim izniniz yok.");

        urun.UrunAdi = dto.UrunAdi;
        urun.Aciklama = dto.Aciklama;
        urun.Kategori = dto.Kategori;
        urun.Birim = dto.Birim;
        urun.BirimFiyat = dto.BirimFiyat;
        urun.Barkod = dto.Barkod;

        await _urunRepo.GuncelleAsync(urun);

        var toplamStok = await _stokRepo.ToplamStokGetirAsync(urun.Id, dto.CompanyId);
        return UrunToDto(urun, toplamStok);
    }

    public async Task<bool> SilAsync(UrunSilDto dto)
    {
        var urun = await _urunRepo.GetByIdAsync(dto.Id, dto.CompanyId)
            ?? throw new KeyNotFoundException("Ürün bulunamadı.");

        if (urun.CompanyId != dto.CompanyId)
            throw new UnauthorizedAccessException("Bu ürüne erişim izniniz yok.");

        urun.IsDeleted = true;
        await _urunRepo.GuncelleAsync(urun);
        return true;
    }

    public async Task<List<string>> KategorileriGetirAsync(string companyId)
    {
        return await _urunRepo.KategorileriGetirAsync(companyId);
    }

    private static UrunDto UrunToDto(Urun u, int toplamStok) => new()
    {
        Id = u.Id,
        CompanyId = u.CompanyId,
        UrunKodu = u.UrunKodu,
        UrunAdi = u.UrunAdi,
        Aciklama = u.Aciklama,
        Kategori = u.Kategori,
        Birim = u.Birim,
        BirimFiyat = u.BirimFiyat,
        Barkod = u.Barkod,
        OlusturmaTarihi = u.OlusturmaTarihi,
        GuncellemeTarihi = u.GuncellemeTarihi,
        ToplamStok = toplamStok
    };
}
