using DepoYonetimi.API.Entities;

namespace DepoYonetimi.API.Repositories.Interfaces;

public interface IUrunRepository
{
    Task<Urun?> GetByIdAsync(int id, string companyId);
    Task<(List<Urun> Liste, int ToplamSayisi)> ListeGetirAsync(string companyId, string? arama, string? kategori, int sayfa, int sayfaBoyutu);
    Task<bool> KodVarMiAsync(string urunKodu, string companyId, int? haricId = null);
    Task<Urun> EkleAsync(Urun urun);
    Task GuncelleAsync(Urun urun);
    Task<List<string>> KategorileriGetirAsync(string companyId);
}
