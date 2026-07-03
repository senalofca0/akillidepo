namespace DepoYonetimi.API.Entities;

/// <summary>
/// Fiziksel depo lokasyonları. Raf ve bölge bazında organize edilir.
/// </summary>
public class Depo : TemelVarlik
{
    public string DepoKodu { get; set; } = string.Empty;
    public string DepoAdi { get; set; } = string.Empty;
    public string? Konum { get; set; } // Şehir / adres
    public string Bolge { get; set; } = string.Empty; // A, B, C bölgesi
    public string Raf { get; set; } = string.Empty;   // Raf-01, Raf-02 vs
    public int Kapasite { get; set; } = 0; // Maksimum stok kapasitesi
    public bool AktifMi { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Stok> Stoklar { get; set; } = new List<Stok>();
    public virtual ICollection<DepoHareket> Hareketler { get; set; } = new List<DepoHareket>();
}
