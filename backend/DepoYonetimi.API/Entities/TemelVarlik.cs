namespace DepoYonetimi.API.Entities;

/// <summary>
/// Tüm entity'lerin kalıtım aldığı temel sınıf.
/// Audit, soft-delete ve multi-tenant alanlarını merkezi olarak tanımlar.
/// </summary>
public abstract class TemelVarlik
{
    public int Id { get; set; }
    public string CompanyId { get; set; } = string.Empty;

    // ─── Aktif/Pasif — silmeden devre dışı bırakma ─────────────────────────
    public bool IsActive { get; set; } = true;

    // ─── Soft Delete ────────────────────────────────────────────────────────
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    // ─── Audit ──────────────────────────────────────────────────────────────
    public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
    public string? OlusturanKisi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    public string? GuncelleyenKisi { get; set; }

    // ─── Optimistic Concurrency ─────────────────────────────────────────────
    // Aynı kaydı iki kullanıcı aynı anda güncellemeye çalışırsa EF Core hata fırlatır
    [System.ComponentModel.DataAnnotations.Timestamp]
    public byte[]? RowVersion { get; set; }

    // ─── Versiyon Sayacı ────────────────────────────────────────────────────
    // Kayıt kaç kez güncellendi?
    public int Version { get; set; } = 1;
}
