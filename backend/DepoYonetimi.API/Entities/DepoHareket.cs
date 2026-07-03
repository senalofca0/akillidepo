namespace DepoYonetimi.API.Entities;

/// <summary>
/// Depo giriş ve çıkış hareketlerini kaydeder.
/// Her hareket sonrası stok otomatik güncellenir.
/// </summary>
public class DepoHareket : TemelVarlik
{
    public int UrunId { get; set; }
    public int DepoId { get; set; }
    public string HareketTipi { get; set; } = string.Empty; // Giris, Cikis
    public int Miktar { get; set; } = 0;
    public DateTime HareketTarihi { get; set; } = DateTime.UtcNow;
    public string? Aciklama { get; set; }
    public string? ReferansNo { get; set; } // Sipariş no, fatura no vs.
    public string IslemYapan { get; set; } = string.Empty; // Kullanıcı adı

    // Navigation properties
    public virtual Urun Urun { get; set; } = null!;
    public virtual Depo Depo { get; set; } = null!;
}
