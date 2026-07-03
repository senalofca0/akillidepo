using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Depo;
using DepoYonetimi.API.Entities;
using DepoYonetimi.API.Managers.Interfaces;
using DepoYonetimi.API.Repositories.Interfaces;

namespace DepoYonetimi.API.Managers;

public class DepoManager : IDepoManager
{
    private readonly IDepoRepository _depoRepo;
    private readonly IStokRepository _stokRepo;

    public DepoManager(IDepoRepository depoRepo, IStokRepository stokRepo)
    {
        _depoRepo = depoRepo;
        _stokRepo = stokRepo;
    }

    public async Task<DepoDto?> GetByIdAsync(int id, string companyId)
    {
        var depo = await _depoRepo.GetByIdAsync(id, companyId);
        if (depo == null) return null;

        var (stoklar, _) = await _stokRepo.ListeGetirAsync(companyId, null, id, 1, 9999);
        return DepoToDto(depo, stoklar.Sum(s => s.Miktar), stoklar.Count);
    }

    public async Task<PaginatedResponse<DepoDto>> ListeGetirAsync(string companyId, string? arama, int sayfa, int sayfaBoyutu)
    {
        var (liste, toplamSayisi) = await _depoRepo.ListeGetirAsync(companyId, arama, sayfa, sayfaBoyutu);

        var dtoListesi = new List<DepoDto>();
        foreach (var depo in liste)
        {
            var (stoklar, _) = await _stokRepo.ListeGetirAsync(companyId, null, depo.Id, 1, 9999);
            dtoListesi.Add(DepoToDto(depo, stoklar.Sum(s => s.Miktar), stoklar.Count));
        }

        return new PaginatedResponse<DepoDto>
        {
            Data = dtoListesi,
            TotalCount = toplamSayisi,
            Page = sayfa,
            PageSize = sayfaBoyutu,
            TotalPages = (int)Math.Ceiling((double)toplamSayisi / sayfaBoyutu)
        };
    }

    public async Task<List<DepoDto>> HepsiniGetirAsync(string companyId)
    {
        var depolar = await _depoRepo.HepsiniGetirAsync(companyId);
        return depolar.Select(d => DepoToDto(d, 0, 0)).ToList();
    }

    public async Task<DepoDto> OlusturAsync(DepoOlusturDto dto)
    {
        if (await _depoRepo.KodVarMiAsync(dto.DepoKodu, dto.CompanyId))
            throw new InvalidOperationException($"'{dto.DepoKodu}' depo kodu zaten kullanımda.");

        var depo = new Depo
        {
            CompanyId = dto.CompanyId,
            DepoKodu = dto.DepoKodu.ToUpperInvariant(),
            DepoAdi = dto.DepoAdi,
            Konum = dto.Konum,
            Bolge = dto.Bolge,
            Raf = dto.Raf,
            Kapasite = dto.Kapasite,
            AktifMi = dto.AktifMi,
            OlusturmaTarihi = DateTime.UtcNow
        };

        var eklenen = await _depoRepo.EkleAsync(depo);
        return DepoToDto(eklenen, 0, 0);
    }

    public async Task<DepoDto> GuncelleAsync(DepoGuncelleDto dto)
    {
        var depo = await _depoRepo.GetByIdAsync(dto.Id, dto.CompanyId)
            ?? throw new KeyNotFoundException("Depo bulunamadı.");

        if (depo.CompanyId != dto.CompanyId)
            throw new UnauthorizedAccessException("Bu depoya erişim izniniz yok.");

        depo.DepoAdi = dto.DepoAdi;
        depo.Konum = dto.Konum;
        depo.Bolge = dto.Bolge;
        depo.Raf = dto.Raf;
        depo.Kapasite = dto.Kapasite;
        depo.AktifMi = dto.AktifMi;

        await _depoRepo.GuncelleAsync(depo);

        var (stoklar, _) = await _stokRepo.ListeGetirAsync(dto.CompanyId, null, dto.Id, 1, 9999);
        return DepoToDto(depo, stoklar.Sum(s => s.Miktar), stoklar.Count);
    }

    public async Task<bool> SilAsync(DepoSilDto dto)
    {
        var depo = await _depoRepo.GetByIdAsync(dto.Id, dto.CompanyId)
            ?? throw new KeyNotFoundException("Depo bulunamadı.");

        if (depo.CompanyId != dto.CompanyId)
            throw new UnauthorizedAccessException("Bu depoya erişim izniniz yok.");

        depo.IsDeleted = true;
        await _depoRepo.GuncelleAsync(depo);
        return true;
    }

    private static DepoDto DepoToDto(Depo d, int toplamStok, int urunCesidi) => new()
    {
        Id = d.Id,
        CompanyId = d.CompanyId,
        DepoKodu = d.DepoKodu,
        DepoAdi = d.DepoAdi,
        Konum = d.Konum,
        Bolge = d.Bolge,
        Raf = d.Raf,
        Kapasite = d.Kapasite,
        AktifMi = d.AktifMi,
        ToplamStok = toplamStok,
        UrunCesidi = urunCesidi,
        OlusturmaTarihi = d.OlusturmaTarihi
    };
}
