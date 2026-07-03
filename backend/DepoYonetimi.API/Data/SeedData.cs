using DepoYonetimi.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepoYonetimi.API.Data;

public static class SeedData
{
    public static async Task YukleAsync(DepoDbContext context)
    {
        if (await context.Urunler.AnyAsync()) return;

        await YukleUretim(context);   // DEMO-001: ABC Üretim A.Ş.
        await YukleEczane(context);   // DEMO-002: Medi-Raf Eczane Zinciri
    }

    // ─── SENARYO 1: Fabrika / Üretim ─────────────────────────────────────────
    // Hammadde deposu, yarı mamul deposu, bitmiş ürün deposu ayrı ayrı
    // Kritik stok = üretim durabilir
    private static async Task YukleUretim(DepoDbContext context)
    {
        const string cid = "DEMO-001";

        var depolar = new List<Depo>
        {
            new() { CompanyId = cid, DepoKodu = "HM-01", DepoAdi = "Hammadde Deposu",      Bolge = "A", Raf = "Raf-01", Konum = "Fabrika - Giriş Katı", Kapasite = 10000, AktifMi = true,  OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, DepoKodu = "YM-01", DepoAdi = "Yarı Mamul Deposu",    Bolge = "B", Raf = "Raf-02", Konum = "Fabrika - Üretim Katı", Kapasite = 5000,  AktifMi = true,  OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, DepoKodu = "BU-01", DepoAdi = "Bitmiş Ürün Deposu",   Bolge = "C", Raf = "Raf-03", Konum = "Fabrika - Sevkiyat",    Kapasite = 8000,  AktifMi = true,  OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, DepoKodu = "YD-01", DepoAdi = "Yedek Parça Deposu",   Bolge = "D", Raf = "Raf-04", Konum = "Fabrika - Bakım Birimi", Kapasite = 2000,  AktifMi = false, OlusturmaTarihi = DateTime.UtcNow },
        };
        context.Depolar.AddRange(depolar);
        await context.SaveChangesAsync();

        var urunler = new List<Urun>
        {
            new() { CompanyId = cid, UrunKodu = "HM-001", UrunAdi = "Çelik Sac 2mm",        Kategori = "Hammadde",     Birim = "Kg",   BirimFiyat = 28m,   Aciklama = "Üretim hattı için",    OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "HM-002", UrunAdi = "Alüminyum Profil",      Kategori = "Hammadde",     Birim = "Metre", BirimFiyat = 85m,  Aciklama = "A tipi profil",         OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "YM-001", UrunAdi = "Kesim Parçası A",       Kategori = "Yarı Mamul",   Birim = "Adet", BirimFiyat = 120m,  Aciklama = "Hat-1 çıktısı",        OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "YM-002", UrunAdi = "Montaj Alt Grubu",      Kategori = "Yarı Mamul",   Birim = "Adet", BirimFiyat = 340m,  Aciklama = "Hat-2 çıktısı",        OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "BU-001", UrunAdi = "Model-X Ürün (Paket)",  Kategori = "Bitmiş Ürün",  Birim = "Kutu", BirimFiyat = 1200m, Aciklama = "Sevkiyata hazır",      OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "YP-001", UrunAdi = "Konveyör Kayışı",       Kategori = "Yedek Parça",  Birim = "Adet", BirimFiyat = 450m,  Aciklama = "Bakım yedek parçası",  OlusturmaTarihi = DateTime.UtcNow },
        };
        context.Urunler.AddRange(urunler);
        await context.SaveChangesAsync();

        var stoklar = new List<Stok>
        {
            new() { CompanyId = cid, UrunId = urunler[0].Id, DepoId = depolar[0].Id, Miktar = 4500, MinimumStokSeviyesi = 500,  MaksimumStokSeviyesi = 10000, OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunId = urunler[1].Id, DepoId = depolar[0].Id, Miktar = 180,  MinimumStokSeviyesi = 200,  MaksimumStokSeviyesi = 2000,  OlusturmaTarihi = DateTime.UtcNow }, // KRİTİK
            new() { CompanyId = cid, UrunId = urunler[2].Id, DepoId = depolar[1].Id, Miktar = 320,  MinimumStokSeviyesi = 100,  MaksimumStokSeviyesi = 1000,  OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunId = urunler[3].Id, DepoId = depolar[1].Id, Miktar = 45,   MinimumStokSeviyesi = 50,   MaksimumStokSeviyesi = 500,   OlusturmaTarihi = DateTime.UtcNow }, // KRİTİK
            new() { CompanyId = cid, UrunId = urunler[4].Id, DepoId = depolar[2].Id, Miktar = 210,  MinimumStokSeviyesi = 30,   MaksimumStokSeviyesi = 500,   OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunId = urunler[5].Id, DepoId = depolar[0].Id, Miktar = 12,   MinimumStokSeviyesi = 5,    MaksimumStokSeviyesi = 50,    OlusturmaTarihi = DateTime.UtcNow },
        };
        context.Stoklar.AddRange(stoklar);
        await context.SaveChangesAsync();

        var hareketler = new List<DepoHareket>
        {
            new() { CompanyId = cid, UrunId = urunler[0].Id, DepoId = depolar[0].Id, HareketTipi = "Giris", Miktar = 5000, HareketTarihi = DateTime.UtcNow.AddDays(-7),  IslemYapan = "Ahmet K.",  ReferansNo = "SİPARİŞ-2026-041", OlusturmaTarihi = DateTime.UtcNow.AddDays(-7) },
            new() { CompanyId = cid, UrunId = urunler[0].Id, DepoId = depolar[0].Id, HareketTipi = "Cikis", Miktar = 500,  HareketTarihi = DateTime.UtcNow.AddDays(-5),  IslemYapan = "Üretim Hattı", Aciklama = "Hat-1 üretim tüketimi",  OlusturmaTarihi = DateTime.UtcNow.AddDays(-5) },
            new() { CompanyId = cid, UrunId = urunler[2].Id, DepoId = depolar[1].Id, HareketTipi = "Giris", Miktar = 400,  HareketTarihi = DateTime.UtcNow.AddDays(-4),  IslemYapan = "Mehmet D.", ReferansNo = "ÜRETİM-2026-118",  OlusturmaTarihi = DateTime.UtcNow.AddDays(-4) },
            new() { CompanyId = cid, UrunId = urunler[3].Id, DepoId = depolar[1].Id, HareketTipi = "Giris", Miktar = 80,   HareketTarihi = DateTime.UtcNow.AddDays(-3),  IslemYapan = "Mehmet D.", ReferansNo = "ÜRETİM-2026-119",  OlusturmaTarihi = DateTime.UtcNow.AddDays(-3) },
            new() { CompanyId = cid, UrunId = urunler[3].Id, DepoId = depolar[1].Id, HareketTipi = "Cikis", Miktar = 35,   HareketTarihi = DateTime.UtcNow.AddDays(-1),  IslemYapan = "Üretim Hattı", Aciklama = "Hat-2 montaj tüketimi",  OlusturmaTarihi = DateTime.UtcNow.AddDays(-1) },
            new() { CompanyId = cid, UrunId = urunler[4].Id, DepoId = depolar[2].Id, HareketTipi = "Giris", Miktar = 210,  HareketTarihi = DateTime.UtcNow.AddHours(-6), IslemYapan = "Sevkiyat",  ReferansNo = "SEVKİYAT-2026-077", OlusturmaTarihi = DateTime.UtcNow.AddHours(-6) },
        };
        context.DepoHareketleri.AddRange(hareketler);
        await context.SaveChangesAsync();
    }

    // ─── SENARYO 2: Eczane Zinciri ───────────────────────────────────────────
    // Raf bazlı depolama, ilaç kategorileri, son kullanma tarihi takibi
    private static async Task YukleEczane(DepoDbContext context)
    {
        const string cid = "DEMO-002";

        var depolar = new List<Depo>
        {
            new() { CompanyId = cid, DepoKodu = "ECZ-IST", DepoAdi = "İstanbul Merkez Eczane", Bolge = "A", Raf = "Raf-Reçeteli",   Konum = "İstanbul, Kadıköy", Kapasite = 3000, AktifMi = true,  OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, DepoKodu = "ECZ-ANK", DepoAdi = "Ankara Şube Eczane",     Bolge = "B", Raf = "Raf-OTC",        Konum = "Ankara, Çankaya",   Kapasite = 2000, AktifMi = true,  OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, DepoKodu = "ECZ-DEP", DepoAdi = "Merkez Depo",             Bolge = "C", Raf = "Raf-Soğuk",     Konum = "İstanbul, Esenyurt", Kapasite = 5000, AktifMi = true,  OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, DepoKodu = "ECZ-IZM", DepoAdi = "İzmir Şube Eczane",      Bolge = "D", Raf = "Raf-Medikal",   Konum = "İzmir, Konak",      Kapasite = 1500, AktifMi = false, OlusturmaTarihi = DateTime.UtcNow },
        };
        context.Depolar.AddRange(depolar);
        await context.SaveChangesAsync();

        var urunler = new List<Urun>
        {
            new() { CompanyId = cid, UrunKodu = "ILC-001", UrunAdi = "Augmentin 1000mg",    Kategori = "Antibiyotik",     Birim = "Kutu",  BirimFiyat = 185m, Barkod = "8699514010014", OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "ILC-002", UrunAdi = "Parol 500mg",          Kategori = "Ağrı Kesici",     Birim = "Kutu",  BirimFiyat = 28m,  Barkod = "8699587020018", OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "ILC-003", UrunAdi = "Lantus Solostar",      Kategori = "Diyabet / Soğuk", Birim = "Kalem", BirimFiyat = 420m, Barkod = "8699540030021", Aciklama = "Soğuk zincir ürünü +2°C/+8°C", OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "MED-001", UrunAdi = "Tansiyon Aleti",       Kategori = "Medikal Cihaz",   Birim = "Adet",  BirimFiyat = 850m, Barkod = "8690123110041", OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "SAR-001", UrunAdi = "Steril Gazlı Bez 10lu", Kategori = "Sarf Malzeme",  Birim = "Paket", BirimFiyat = 22m,  Barkod = "8690123120051", OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunKodu = "VIT-001", UrunAdi = "D-Vit 1000 IU",        Kategori = "Vitamin / OTC",   Birim = "Kutu",  BirimFiyat = 95m,  Barkod = "8690123130061", OlusturmaTarihi = DateTime.UtcNow },
        };
        context.Urunler.AddRange(urunler);
        await context.SaveChangesAsync();

        var stoklar = new List<Stok>
        {
            new() { CompanyId = cid, UrunId = urunler[0].Id, DepoId = depolar[0].Id, Miktar = 48,  MinimumStokSeviyesi = 10, MaksimumStokSeviyesi = 200, OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunId = urunler[1].Id, DepoId = depolar[0].Id, Miktar = 7,   MinimumStokSeviyesi = 20, MaksimumStokSeviyesi = 300, OlusturmaTarihi = DateTime.UtcNow }, // KRİTİK
            new() { CompanyId = cid, UrunId = urunler[1].Id, DepoId = depolar[1].Id, Miktar = 85,  MinimumStokSeviyesi = 15, MaksimumStokSeviyesi = 200, OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunId = urunler[2].Id, DepoId = depolar[2].Id, Miktar = 24,  MinimumStokSeviyesi = 5,  MaksimumStokSeviyesi = 100, OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunId = urunler[3].Id, DepoId = depolar[1].Id, Miktar = 3,   MinimumStokSeviyesi = 5,  MaksimumStokSeviyesi = 30,  OlusturmaTarihi = DateTime.UtcNow }, // KRİTİK
            new() { CompanyId = cid, UrunId = urunler[4].Id, DepoId = depolar[0].Id, Miktar = 150, MinimumStokSeviyesi = 20, MaksimumStokSeviyesi = 500, OlusturmaTarihi = DateTime.UtcNow },
            new() { CompanyId = cid, UrunId = urunler[5].Id, DepoId = depolar[1].Id, Miktar = 62,  MinimumStokSeviyesi = 10, MaksimumStokSeviyesi = 200, OlusturmaTarihi = DateTime.UtcNow },
        };
        context.Stoklar.AddRange(stoklar);
        await context.SaveChangesAsync();

        var hareketler = new List<DepoHareket>
        {
            new() { CompanyId = cid, UrunId = urunler[0].Id, DepoId = depolar[0].Id, HareketTipi = "Giris", Miktar = 60,  HareketTarihi = DateTime.UtcNow.AddDays(-6), IslemYapan = "Ecz. Ayşe T.",  ReferansNo = "TESLİMAT-0441", OlusturmaTarihi = DateTime.UtcNow.AddDays(-6) },
            new() { CompanyId = cid, UrunId = urunler[0].Id, DepoId = depolar[0].Id, HareketTipi = "Cikis", Miktar = 12,  HareketTarihi = DateTime.UtcNow.AddDays(-4), IslemYapan = "Ecz. Ayşe T.",  Aciklama = "Reçete satışı",   OlusturmaTarihi = DateTime.UtcNow.AddDays(-4) },
            new() { CompanyId = cid, UrunId = urunler[1].Id, DepoId = depolar[0].Id, HareketTipi = "Giris", Miktar = 100, HareketTarihi = DateTime.UtcNow.AddDays(-5), IslemYapan = "Ecz. Murat Y.", ReferansNo = "TESLİMAT-0442", OlusturmaTarihi = DateTime.UtcNow.AddDays(-5) },
            new() { CompanyId = cid, UrunId = urunler[1].Id, DepoId = depolar[0].Id, HareketTipi = "Cikis", Miktar = 93,  HareketTarihi = DateTime.UtcNow.AddDays(-2), IslemYapan = "Ecz. Ayşe T.",  Aciklama = "OTC satışı - yoğun dönem", OlusturmaTarihi = DateTime.UtcNow.AddDays(-2) },
            new() { CompanyId = cid, UrunId = urunler[2].Id, DepoId = depolar[2].Id, HareketTipi = "Giris", Miktar = 24,  HareketTarihi = DateTime.UtcNow.AddDays(-3), IslemYapan = "Ecz. Murat Y.", ReferansNo = "SOĞUK-2026-018", Aciklama = "Soğuk zincir teslim",  OlusturmaTarihi = DateTime.UtcNow.AddDays(-3) },
            new() { CompanyId = cid, UrunId = urunler[4].Id, DepoId = depolar[0].Id, HareketTipi = "Giris", Miktar = 200, HareketTarihi = DateTime.UtcNow.AddDays(-1), IslemYapan = "Ecz. Murat Y.", ReferansNo = "TESLİMAT-0445", OlusturmaTarihi = DateTime.UtcNow.AddDays(-1) },
            new() { CompanyId = cid, UrunId = urunler[4].Id, DepoId = depolar[0].Id, HareketTipi = "Cikis", Miktar = 50,  HareketTarihi = DateTime.UtcNow.AddHours(-4), IslemYapan = "Ecz. Ayşe T.", Aciklama = "Sube transferi - Ankara",  OlusturmaTarihi = DateTime.UtcNow.AddHours(-4) },
        };
        context.DepoHareketleri.AddRange(hareketler);
        await context.SaveChangesAsync();
    }
}
