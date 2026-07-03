namespace DepoYonetimi.API.DTOs.Depo;

public class DepoDto
{
    public int Id { get; set; }
    public string CompanyId { get; set; } = string.Empty;
    public string DepoKodu { get; set; } = string.Empty;
    public string DepoAdi { get; set; } = string.Empty;
    public string? Konum { get; set; }
    public string Bolge { get; set; } = string.Empty;
    public string Raf { get; set; } = string.Empty;
    public int Kapasite { get; set; }
    public bool AktifMi { get; set; }
    public int ToplamStok { get; set; }    // Depodaki toplam ürün adedi
    public int UrunCesidi { get; set; }   // Farklı ürün sayısı
    public DateTime OlusturmaTarihi { get; set; }
}
