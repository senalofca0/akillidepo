using DepoYonetimi.API.Entities;

namespace DepoYonetimi.API.Repositories.Interfaces;

public interface IHareketRepository
{
    Task<(List<DepoHareket> Liste, int ToplamSayisi)> ListeGetirAsync(
        string companyId, int? urunId, int? depoId,
        string? hareketTipi, DateTime? baslangic, DateTime? bitis,
        int sayfa, int sayfaBoyutu);
    Task<DepoHareket> EkleAsync(DepoHareket hareket);
}
