namespace DepoYonetimi.API.DTOs.Hareket;

public class HareketDto
{
    public int Id { get; set; }
    public string CompanyId { get; set; } = string.Empty;
    public int UrunId { get; set; }
    public string UrunKodu { get; set; } = string.Empty;
    public string UrunAdi { get; set; } = string.Empty;
    public int DepoId { get; set; }
    public string DepoKodu { get; set; } = string.Empty;
    public string DepoAdi { get; set; } = string.Empty;
    public string HareketTipi { get; set; } = string.Empty;
    public int Miktar { get; set; }
    public DateTime HareketTarihi { get; set; }
    public string? Aciklama { get; set; }
    public string? ReferansNo { get; set; }
    public string IslemYapan { get; set; } = string.Empty;
}
