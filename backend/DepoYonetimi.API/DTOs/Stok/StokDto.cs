namespace DepoYonetimi.API.DTOs.Stok;

public class StokDto
{
    public int Id { get; set; }
    public string CompanyId { get; set; } = string.Empty;
    public int UrunId { get; set; }
    public string UrunKodu { get; set; } = string.Empty;
    public string UrunAdi { get; set; } = string.Empty;
    public string Kategori { get; set; } = string.Empty;
    public int DepoId { get; set; }
    public string DepoKodu { get; set; } = string.Empty;
    public string DepoAdi { get; set; } = string.Empty;
    public string Bolge { get; set; } = string.Empty;
    public string Raf { get; set; } = string.Empty;
    public int Miktar { get; set; }
    public int MinimumStokSeviyesi { get; set; }
    public int MaksimumStokSeviyesi { get; set; }
    public bool KritikSeviyede { get; set; } // Miktar <= Minimum
}
