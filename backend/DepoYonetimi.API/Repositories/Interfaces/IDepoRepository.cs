using DepoYonetimi.API.Entities;

namespace DepoYonetimi.API.Repositories.Interfaces;

public interface IDepoRepository
{
    Task<Depo?> GetByIdAsync(int id, string companyId);
    Task<(List<Depo> Liste, int ToplamSayisi)> ListeGetirAsync(string companyId, string? arama, int sayfa, int sayfaBoyutu);
    Task<List<Depo>> HepsiniGetirAsync(string companyId);
    Task<bool> KodVarMiAsync(string depoKodu, string companyId, int? haricId = null);
    Task<Depo> EkleAsync(Depo depo);
    Task GuncelleAsync(Depo depo);
}
