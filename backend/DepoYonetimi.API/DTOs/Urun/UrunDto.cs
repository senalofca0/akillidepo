namespace DepoYonetimi.API.DTOs.Urun;

public class UrunDto
{
    public int Id { get; set; }
    public string CompanyId { get; set; } = string.Empty;
    public string UrunKodu { get; set; } = string.Empty;
    public string UrunAdi { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public string Kategori { get; set; } = string.Empty;
    public string Birim { get; set; } = string.Empty;
    public decimal? BirimFiyat { get; set; }
    public string? Barkod { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    public int ToplamStok { get; set; } // Tüm depolardaki toplam stok
}
