using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Urun;

namespace DepoYonetimi.API.Managers.Interfaces;

public interface IUrunManager
{
    Task<UrunDto?> GetByIdAsync(int id, string companyId);
    Task<PaginatedResponse<UrunDto>> ListeGetirAsync(UrunListeFiltresiDto filtre);
    Task<UrunDto> OlusturAsync(UrunOlusturDto dto);
    Task<UrunDto> GuncelleAsync(UrunGuncelleDto dto);
    Task<bool> SilAsync(UrunSilDto dto);
    Task<List<string>> KategorileriGetirAsync(string companyId);
}
