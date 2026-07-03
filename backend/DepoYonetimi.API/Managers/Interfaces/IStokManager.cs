using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Stok;

namespace DepoYonetimi.API.Managers.Interfaces;

public interface IStokManager
{
    Task<PaginatedResponse<StokDto>> ListeGetirAsync(string companyId, int? urunId, int? depoId, int sayfa, int sayfaBoyutu);
    Task<object> OzetGetirAsync(string companyId);
}
