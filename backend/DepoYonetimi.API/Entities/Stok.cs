namespace DepoYonetimi.API.Entities;

/// <summary>
/// Güncel stok durumu. Her ürünün her depodaki stok seviyesini tutar.
/// </summary>
public class Stok : TemelVarlik
{
    public int UrunId { get; set; }
    public int DepoId { get; set; }
    public int Miktar { get; set; } = 0;
    public int MinimumStokSeviyesi { get; set; } = 0; // İkaz seviyesi
    public int MaksimumStokSeviyesi { get; set; } = 0; // Hedef seviye

    // Navigation properties
    public virtual Urun Urun { get; set; } = null!;
    public virtual Depo Depo { get; set; } = null!;
}
