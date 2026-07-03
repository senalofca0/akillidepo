# Depo Yönetimi Sistemi

Çok şirketli (multi-tenant) depo, stok ve hareket takibini tek ekrandan yönetmek için geliştirilmiş full-stack bir web uygulamasıdır.

---

## Proje Amacı

Farklı sektörlerdeki şirketlerin (üretim, eczane, lojistik vb.) depolarını ve ürün stoklarını merkezi olarak yönetmesini sağlar. Kullanıcılar ürün tanımları yapabilir, depolara stok atayabilir, giriş/çıkış hareketleri girebilir ve stok seviyelerini anlık olarak takip edebilir.

---

## Teknoloji Yığını

| Katman    | Teknoloji                                    |
|-----------|----------------------------------------------|
| Backend   | .NET 9 / ASP.NET Core Web API                |
| ORM       | Entity Framework Core 9 (SQL Server)         |
| Dokümantasyon | Swagger / OpenAPI (Swashbuckle)          |
| Frontend  | React 18 + TypeScript + Vite                 |
| UI        | Material UI (MUI v5)                         |
| Veritabanı | Microsoft SQL Server                        |

---

## Proje Yapısı

```
mt/
├── backend/
│   └── DepoYonetimi.API/
│       ├── Controllers/       # API endpoint'leri
│       ├── Entities/          # Veritabanı entity sınıfları
│       ├── DTOs/              # Veri transfer nesneleri
│       ├── Managers/          # İş mantığı katmanı
│       ├── Data/              # DbContext ve seed verisi
│       └── Migrations/        # EF Core migration dosyaları
└── frontend/
    └── depo-yonetimi/
        └── src/
            ├── sayfalar/      # Sayfa bileşenleri
            ├── bilesenler/    # Yeniden kullanılabilir UI bileşenleri
            ├── servisler/     # API istemci servisleri
            └── tipler/        # TypeScript tip tanımları
```

---

## Veritabanı Tablo Yapısı

### TemelVarlik (Tüm tablolara kalıtılan taban sınıf)

| Alan               | Tip        | Açıklama                                  |
|--------------------|------------|-------------------------------------------|
| Id                 | int        | Primary key                               |
| CompanyId          | string     | Multi-tenant şirket kimliği               |
| IsActive           | bool       | Kayıt aktif/pasif durumu                  |
| IsDeleted          | bool       | Soft delete flag'i                        |
| DeletedAt          | DateTime?  | Silinme tarihi                            |
| DeletedBy          | string?    | Silen kullanıcı                           |
| OlusturmaTarihi    | DateTime   | Kayıt oluşturma tarihi                    |
| OlusturanKisi      | string?    | Kaydı oluşturan kullanıcı                 |
| GuncellemeTarihi   | DateTime?  | Son güncelleme tarihi                     |
| GuncelleyenKisi    | string?    | Son güncelleyen kullanıcı                 |
| RowVersion         | byte[]?    | Optimistic concurrency için timestamp     |
| Version            | int        | Kaç kez güncellendiğini tutan sayaç       |

---

### Urunler

| Alan           | Tip       | Açıklama                          |
|----------------|-----------|-----------------------------------|
| UrunKodu       | string    | Benzersiz ürün kodu               |
| UrunAdi        | string    | Ürün adı                          |
| Aciklama       | string?   | Ürün açıklaması                   |
| Kategori       | string    | Ürün kategorisi                   |
| Birim          | string    | Ölçü birimi (Adet, Kg, Lt vb.)   |
| BirimFiyat     | decimal?  | Birim fiyatı                      |
| Barkod         | string?   | Barkod numarası                   |
| AgirlikKg      | decimal?  | Ürün ağırlığı (lojistik/kargo)   |
| TedarikciKodu  | string?   | Tedarikçinin kendi ürün kodu      |
| RafOmruGun     | int?      | Raf ömrü — gıda/ilaç için (gün)  |
| ResimUrl       | string?   | Ürün görseli URL'i                |

---

### Depolar

| Alan       | Tip     | Açıklama                            |
|------------|---------|-------------------------------------|
| DepoKodu   | string  | Benzersiz depo kodu                 |
| DepoAdi    | string  | Depo adı                            |
| Konum      | string? | Şehir / adres                       |
| Bolge      | string  | Bölge (A, B, C vb.)                |
| Raf        | string  | Raf tanımı (Raf-01, Raf-02 vb.)    |
| Kapasite   | int     | Maksimum stok kapasitesi            |
| AktifMi    | bool    | Depo aktif mi?                      |

---

### Stoklar

| Alan                  | Tip  | Açıklama                         |
|-----------------------|------|----------------------------------|
| UrunId                | int  | FK → Urunler                     |
| DepoId                | int  | FK → Depolar                     |
| Miktar                | int  | Güncel stok miktarı              |
| MinimumStokSeviyesi   | int  | İkaz seviyesi (minimum eşik)     |
| MaksimumStokSeviyesi  | int  | Hedef maksimum seviye            |

> Her ürün/depo çifti için tek bir Stok kaydı tutulur. Hareket girildiğinde bu kayıt otomatik güncellenir.

---

### DepoHareketleri

| Alan               | Tip      | Açıklama                                    |
|--------------------|----------|---------------------------------------------|
| UrunId             | int      | FK → Urunler                                |
| DepoId             | int      | FK → Depolar                                |
| HareketTipi        | string   | `Giris` veya `Cikis`                        |
| Miktar             | int      | Hareket miktarı                             |
| HareketTarihi      | DateTime | İşlem tarihi                                |
| Aciklama           | string?  | Açıklama notu                               |
| ReferansNo         | string?  | Sipariş no, fatura no vb.                   |
| IslemYapan         | string   | Hareketi giren kullanıcı adı                |
| IslemNo            | string   | Otomatik üretilen benzersiz işlem numarası  |
| IptalEdildi        | bool     | Bu hareket iptal edildi mi?                 |
| IptalEdenHareketId | int?     | İptal eden karşı hareketin Id'si            |
| IpAdresi           | string?  | İşlemi yapan kullanıcının IP adresi         |

---

## API Endpoint'leri

| Controller  | Prefix          | Temel İşlemler                              |
|-------------|-----------------|---------------------------------------------|
| Urun        | `/api/urun`     | Ürün CRUD, listeleme, filtreleme            |
| Depo        | `/api/depo`     | Depo CRUD, kapasite sorgulama               |
| Stok        | `/api/stok`     | Stok sorgulama, dashboard özeti             |
| Hareket     | `/api/hareket`  | Giriş/çıkış hareketi, hareket geçmişi      |

Swagger UI: `https://localhost:{port}/swagger`

---

## Öne Çıkan Özellikler

-
- **Soft Delete** — Kayıtlar gerçekten silinmez, `IsDeleted` + `DeletedAt` ile işaretlenir
- **Optimistic Concurrency** — `RowVersion` ile aynı anda iki güncelleme çakışması önlenir
- **Hareket İptali** — Her hareket `IptalEdenHareketId` zinciriyle takip edilebilir
- **Stok Uyarıları** — Minimum stok seviyesinin altına düşen ürünler dashboard'da gösterilir
- **Sayfalama** — Tüm liste endpoint'leri `PaginatedResponse<T>` döner
