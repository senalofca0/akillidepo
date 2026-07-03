namespace DepoYonetimi.API.Entities;

/// <summary>
/// Ürün tanımları. Depoya konulacak ürünlerin master data'sını tutar.
/// </summary>
public class Urun : TemelVarlik
{
    public string UrunKodu { get; set; } = string.Empty;
    public string UrunAdi { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public string Kategori { get; set; } = string.Empty;
    public string Birim { get; set; } = "Adet";
    public decimal? BirimFiyat { get; set; }
    public string? Barkod { get; set; }

    // ─── Ek Ürün Bilgileri ──────────────────────────────────────────────────
    public decimal? AgirlikKg { get; set; }         // Ürün ağırlığı (lojistik, kargo hesabı için)
    public string? TedarikciKodu { get; set; }      // Tedarikçinin kendi ürün kodu
    public int? RafOmruGun { get; set; }            // Gıda/ilaç için raf ömrü (gün)
    public string? ResimUrl { get; set; }           // Ürün görseli URL'i

    // Navigation properties
    public virtual ICollection<Stok> Stoklar { get; set; } = new List<Stok>();
    public virtual ICollection<DepoHareket> Hareketler { get; set; } = new List<DepoHareket>();
}
