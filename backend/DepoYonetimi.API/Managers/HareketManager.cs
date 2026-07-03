using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Hareket;
using DepoYonetimi.API.Entities;
using DepoYonetimi.API.Managers.Interfaces;
using DepoYonetimi.API.Repositories.Interfaces;

namespace DepoYonetimi.API.Managers;

public class HareketManager : IHareketManager
{
    private readonly IHareketRepository _hareketRepo;
    private readonly IStokRepository _stokRepo;
    private readonly IUrunRepository _urunRepo;
    private readonly IDepoRepository _depoRepo;

    public HareketManager(
        IHareketRepository hareketRepo,
        IStokRepository stokRepo,
        IUrunRepository urunRepo,
        IDepoRepository depoRepo)
    {
        _hareketRepo = hareketRepo;
        _stokRepo = stokRepo;
        _urunRepo = urunRepo;
        _depoRepo = depoRepo;
    }

    public async Task<PaginatedResponse<HareketDto>> ListeGetirAsync(HareketListeFiltresiDto filtre)
    {
        var (liste, toplamSayisi) = await _hareketRepo.ListeGetirAsync(
            filtre.CompanyId, filtre.UrunId, filtre.DepoId,
            filtre.HareketTipi, filtre.BaslangicTarihi, filtre.BitisTarihi,
            filtre.Sayfa, filtre.SayfaBoyutu);

        var dtoListesi = liste.Select(HareketToDto).ToList();

        return new PaginatedResponse<HareketDto>
        {
            Data = dtoListesi,
            TotalCount = toplamSayisi,
            Page = filtre.Sayfa,
            PageSize = filtre.SayfaBoyutu,
            TotalPages = (int)Math.Ceiling((double)toplamSayisi / filtre.SayfaBoyutu)
        };
    }

    public async Task<HareketDto> OlusturAsync(HareketOlusturDto dto)
    {
        // Ürün ve depo geçerliliğini doğrula
        var urun = await _urunRepo.GetByIdAsync(dto.UrunId, dto.CompanyId)
            ?? throw new KeyNotFoundException("Ürün bulunamadı veya bu şirkete ait değil.");

        var depo = await _depoRepo.GetByIdAsync(dto.DepoId, dto.CompanyId)
            ?? throw new KeyNotFoundException("Depo bulunamadı veya bu şirkete ait değil.");

        if (!depo.AktifMi)
            throw new InvalidOperationException("Seçilen depo aktif değil.");

        // Mevcut stoku bul veya oluştur
        var stok = await _stokRepo.GetByUrunDepoAsync(dto.UrunId, dto.DepoId, dto.CompanyId);

        if (dto.HareketTipi == "Cikis")
        {
            if (stok == null || stok.Miktar < dto.Miktar)
                throw new InvalidOperationException(
                    $"Yetersiz stok. Mevcut: {stok?.Miktar ?? 0}, İstenen: {dto.Miktar}");

            stok.Miktar -= dto.Miktar;
            await _stokRepo.GuncelleAsync(stok);
        }
        else // Giris
        {
            if (stok == null)
            {
                stok = new Stok
                {
                    CompanyId = dto.CompanyId,
                    UrunId = dto.UrunId,
                    DepoId = dto.DepoId,
                    Miktar = dto.Miktar,
                    MinimumStokSeviyesi = 10,
                    MaksimumStokSeviyesi = depo.Kapasite,
                    OlusturmaTarihi = DateTime.UtcNow
                };
                await _stokRepo.EkleAsync(stok);
            }
            else
            {
                stok.Miktar += dto.Miktar;
                await _stokRepo.GuncelleAsync(stok);
            }
        }

        // Hareketi kaydet
        var hareket = new DepoHareket
        {
            CompanyId = dto.CompanyId,
            UrunId = dto.UrunId,
            DepoId = dto.DepoId,
            HareketTipi = dto.HareketTipi,
            Miktar = dto.Miktar,
            HareketTarihi = dto.HareketTarihi ?? DateTime.UtcNow,
            Aciklama = dto.Aciklama,
            ReferansNo = dto.ReferansNo,
            IslemYapan = dto.IslemYapan,
            IslemNo = $"HRK-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
            OlusturmaTarihi = DateTime.UtcNow
        };

        var eklenen = await _hareketRepo.EkleAsync(hareket);
        eklenen.Urun = urun;
        eklenen.Depo = depo;

        return HareketToDto(eklenen);
    }

    private static HareketDto HareketToDto(DepoHareket h) => new()
    {
        Id = h.Id,
        CompanyId = h.CompanyId,
        UrunId = h.UrunId,
        UrunKodu = h.Urun?.UrunKodu ?? string.Empty,
        UrunAdi = h.Urun?.UrunAdi ?? string.Empty,
        DepoId = h.DepoId,
        DepoKodu = h.Depo?.DepoKodu ?? string.Empty,
        DepoAdi = h.Depo?.DepoAdi ?? string.Empty,
        HareketTipi = h.HareketTipi,
        Miktar = h.Miktar,
        HareketTarihi = h.HareketTarihi,
        Aciklama = h.Aciklama,
        ReferansNo = h.ReferansNo,
        IslemYapan = h.IslemYapan
    };
}
