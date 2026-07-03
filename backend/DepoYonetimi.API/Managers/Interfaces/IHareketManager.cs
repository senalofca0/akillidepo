using DepoYonetimi.API.DTOs;
using DepoYonetimi.API.DTOs.Hareket;

namespace DepoYonetimi.API.Managers.Interfaces;

public interface IHareketManager
{
    Task<PaginatedResponse<HareketDto>> ListeGetirAsync(HareketListeFiltresiDto filtre);
    Task<HareketDto> OlusturAsync(HareketOlusturDto dto);
}
