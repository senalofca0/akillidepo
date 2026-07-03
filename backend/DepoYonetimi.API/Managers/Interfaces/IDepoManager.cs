using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Depo;

namespace DepoYonetimi.API.Managers.Interfaces;

public interface IDepoManager
{
    Task<DepoDto?> GetByIdAsync(int id, string companyId);
    Task<PaginatedResponse<DepoDto>> ListeGetirAsync(string companyId, string? arama, int sayfa, int sayfaBoyutu);
    Task<List<DepoDto>> HepsiniGetirAsync(string companyId);
    Task<DepoDto> OlusturAsync(DepoOlusturDto dto);
    Task<DepoDto> GuncelleAsync(DepoGuncelleDto dto);
    Task<bool> SilAsync(DepoSilDto dto);
}
