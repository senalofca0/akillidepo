using DepoYonetimi.API.Entities;

namespace DepoYonetimi.API.Repositories.Interfaces;

public interface IStokRepository
{
    Task<Stok?> GetByUrunDepoAsync(int urunId, int depoId, string companyId);
    Task<(List<Stok> Liste, int ToplamSayisi)> ListeGetirAsync(string companyId, int? urunId, int? depoId, int sayfa, int sayfaBoyutu);
    Task<List<Stok>> KritikStokGetirAsync(string companyId);
    Task<int> ToplamStokGetirAsync(int urunId, string companyId);
    Task<Dictionary<int, int>> ToplamStokBulkGetirAsync(List<int> urunIdleri, string companyId);
    Task<Stok> EkleAsync(Stok stok);
    Task GuncelleAsync(Stok stok);
    Task<int> ToplamUrunSayisiAsync(string companyId);
    Task<int> ToplamDepoSayisiAsync(string companyId);
    Task<int> KritikStokSayisiAsync(string companyId);
    Task<int> ToplamHareketSayisiAsync(string companyId);
}
