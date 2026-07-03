# Akıllı Depo Yönetimi — Çalışma Raporu

**Tarih:** 3 Temmuz 2026

---

## Proje Hakkında

Çok şirketli (multi-tenant) depo, stok ve hareket takibini tek ekrandan yönetmek için geliştirilmiş full-stack bir web uygulamasıdır.

Farklı sektörlerdeki şirketlerin (üretim, eczane, lojistik vb.) depolarını ve ürün stoklarını merkezi olarak yönetmesini sağlar. Kullanıcılar ürün tanımları yapabilir, depolara stok atayabilir, giriş/çıkış hareketleri girebilir ve stok seviyelerini anlık olarak takip edebilir.

---

## Ne Yapıldı?

Akıllı Depo Yönetimi modülü verilen teknik gereksinimler ve mimari kurallar doğrultusunda sıfırdan geliştirildi.

- .NET 9 Backend API — Controller → Manager → Repository → Entity katmanlı mimari
- React 18 + TypeScript + Material-UI tek sayfa frontend
- EF Core Migration tabanlı MS SQL Server veritabanı şeması
- Tüm listeleme endpoint'lerinde server-side pagination (arama + filtre desteği)
- Multi-tenant yapı: CompanyId bazlı veri izolasyonu
- Soft delete: IsDeleted ile mantıksal silme, tüm entity'lerde
- Dashboard özet kartları, sayfalanmış tablolar, ekleme/düzenleme/silme modalları
- İki farklı şirket senaryosu için otomatik seed data

---

## Kullanılan Teknolojiler

| Katman           | Teknoloji                       | Versiyon |
|------------------|---------------------------------|----------|
| Backend          | .NET / ASP.NET Core Web API     | 9.0      |
| ORM              | Entity Framework Core           | 9.0.6    |
| Veritabanı       | MS SQL Server                   | -        |
| API Dokümantasyon| Swagger / OpenAPI (Swashbuckle) | 6.9.0    |
| Frontend         | React                           | 18.2.0   |
| Dil              | TypeScript                      | 5.x      |
| Build Tool       | Vite                            | -        |
| UI Kütüphanesi   | Material UI (MUI)               | 5.14.0   |
| HTTP Client      | Axios                           | 1.4.0    |

---

## Proje Yapısı

```
mt/
├── backend/
│   └── DepoYonetimi.API/
│       ├── Controllers/    # API endpoint'leri
│       ├── Entities/       # Veritabanı entity sınıfları
│       ├── DTOs/           # Veri transfer nesneleri
│       ├── Managers/       # İş mantığı katmanı
│       ├── Repositories/   # Veritabanı erişim katmanı
│       ├── Data/           # DbContext ve seed verisi
│       └── Migrations/     # EF Core migration dosyaları
└── frontend/
    └── depo-yonetimi/
        └── src/
            ├── sayfalar/   # Sayfa bileşenleri
            ├── bilesenler/ # Yeniden kullanılabilir UI bileşenleri
            ├── servisler/  # API istemci servisleri
            └── tipler/     # TypeScript tip tanımları
```

---

## Veritabanı Tablo Yapısı

### TemelVarlik — Tüm tablolara kalıtılan taban sınıf

| Alan             | Tip       | Açıklama                              |
|------------------|-----------|---------------------------------------|
| Id               | int       | Primary key                           |
| CompanyId        | string    | Multi-tenant şirket kimliği           |
| IsActive         | bool      | Kayıt aktif/pasif durumu              |
| IsDeleted        | bool      | Soft delete flag'i                    |
| DeletedAt        | DateTime? | Silinme tarihi                        |
| DeletedBy        | string?   | Silen kullanıcı                       |
| OlusturmaTarihi  | DateTime  | Kayıt oluşturma tarihi                |
| OlusturanKisi    | string?   | Kaydı oluşturan kullanıcı             |
| GuncellemeTarihi | DateTime? | Son güncelleme tarihi                 |
| GuncelleyenKisi  | string?   | Son güncelleyen kullanıcı             |
| RowVersion       | byte[]?   | Optimistic concurrency için timestamp |
| Version          | int       | Kaç kez güncellendiğini tutan sayaç   |

### Urunler

| Alan          | Tip      | Açıklama                         |
|---------------|----------|----------------------------------|
| UrunKodu      | string   | Benzersiz ürün kodu              |
| UrunAdi       | string   | Ürün adı                         |
| Aciklama      | string?  | Ürün açıklaması                  |
| Kategori      | string   | Ürün kategorisi                  |
| Birim         | string   | Ölçü birimi (Adet, Kg, Lt vb.)  |
| BirimFiyat    | decimal? | Birim fiyatı                     |
| Barkod        | string?  | Barkod numarası                  |
| AgirlikKg     | decimal? | Ürün ağırlığı (lojistik/kargo)  |
| TedarikciKodu | string?  | Tedarikçinin kendi ürün kodu     |
| RafOmruGun    | int?     | Raf ömrü — gıda/ilaç için (gün) |
| ResimUrl      | string?  | Ürün görseli URL'i               |

### Depolar

| Alan     | Tip     | Açıklama                         |
|----------|---------|----------------------------------|
| DepoKodu | string  | Benzersiz depo kodu              |
| DepoAdi  | string  | Depo adı                         |
| Konum    | string? | Şehir / adres                    |
| Bolge    | string  | Bölge (A, B, C vb.)             |
| Raf      | string  | Raf tanımı (Raf-01, Raf-02 vb.) |
| Kapasite | int     | Maksimum stok kapasitesi         |
| AktifMi  | bool    | Depo aktif mi?                   |

### Stoklar

| Alan                 | Tip | Açıklama                      |
|----------------------|-----|-------------------------------|
| UrunId               | int | FK → Urunler                  |
| DepoId               | int | FK → Depolar                  |
| Miktar               | int | Güncel stok miktarı           |
| MinimumStokSeviyesi  | int | İkaz seviyesi (minimum eşik)  |
| MaksimumStokSeviyesi | int | Hedef maksimum seviye         |

> Her ürün/depo çifti için tek bir Stok kaydı tutulur. Hareket girildiğinde bu kayıt otomatik güncellenir.

### DepoHareketleri

| Alan               | Tip      | Açıklama                                   |
|--------------------|----------|--------------------------------------------|
| UrunId             | int      | FK → Urunler                               |
| DepoId             | int      | FK → Depolar                               |
| HareketTipi        | string   | `Giris` veya `Cikis`                       |
| Miktar             | int      | Hareket miktarı                            |
| HareketTarihi      | DateTime | İşlem tarihi                               |
| Aciklama           | string?  | Açıklama notu                              |
| ReferansNo         | string?  | Sipariş no, fatura no vb.                  |
| IslemYapan         | string   | Hareketi giren kullanıcı adı               |
| IslemNo            | string   | Otomatik üretilen benzersiz işlem numarası |
| IptalEdildi        | bool     | Bu hareket iptal edildi mi?                |
| IptalEdenHareketId | int?     | İptal eden karşı hareketin Id'si           |
| IpAdresi           | string?  | İşlemi yapan kullanıcının IP adresi        |

---

## API Endpoint'leri

| Controller | Prefix         | Temel İşlemler                          |
|------------|----------------|-----------------------------------------|
| Urun       | `/api/urun`    | Ürün CRUD, listeleme, filtreleme        |
| Depo       | `/api/depo`    | Depo CRUD, kapasite sorgulama           |
| Stok       | `/api/stok`    | Stok sorgulama, dashboard özeti         |
| Hareket    | `/api/hareket` | Giriş/çıkış hareketi, hareket geçmişi  |

Swagger UI: `http://localhost:5143/swagger`

---

## Mimari Kararlar

**TemelVarlik soyut sınıfı** — Tüm entity'ler bu sınıftan kalıtım alır. Id, CompanyId, IsDeleted, audit alanları merkezi olarak tanımlanır. Her entity'de tekrar kod yazmaya gerek kalmaz, multi-tenant ve soft-delete garantili olur.

**Stok modeli: anlık durum + hareket geçmişi ayrımı** — Stok tablosu anlık miktarı tutar, DepoHareket tablosu tüm giriş/çıkış tarihçesini saklar. Stok sorgulama hızlı çalışır (aggregate hesaplama yok), hareket geçmişi eksiksiz korunur.

**Repository pattern** — İş mantığı (Manager) ile veritabanı erişimi (Repository) birbirinden ayrıldı. Test edilebilirlik ve katman bağımsızlığı sağlandı.

**Server-side pagination** — Tüm listeleme endpoint'lerinde `CountAsync` + `Skip/Take` kombinasyonu kullanıldı, client-side filtreleme yapılmadı.

---

## Karşılaşılan Sorunlar ve Çözümler

| Sorun | Çözüm |
|-------|-------|
| EF Core migration sonrası `RowVersion` alanı SQL Server'da doğru tipe dönüşmedi | `[Timestamp]` attribute'u `TemelVarlik`'e eklendi, migration yeniden oluşturuldu |
| Frontend'den backend'e istek atılırken CORS hatası alındı | `Program.cs`'e `AddCors` + `UseCors` politikası eklendi |
| Stok güncelleme işlemlerinde değişiklikler kaydedilmiyordu | Tüm `GuncelleAsync` metodlarına `_context.Entry(entity).State = EntityState.Modified` eklendi |
| Ürün listesinde her satır için ayrı DB sorgusu atılıyordu (N+1) | `GroupBy` ile tek sorguda tüm stok toplamları çekilecek şekilde düzeltildi |

---

## Yapay Zeka Kullanımı

**Araç:** ChatGPT ve Kiro (AI destekli IDE)

| Aşama | Açıklama |
|-------|----------|
| Frontend geliştirme | MUI bileşen yapıları, TypeScript tip tanımları ve servis katmanı kodları yapay zeka desteğiyle oluşturuldu |
| Seed data | Gerçekçi örnek veri üretimi için kullanıldı |
| Hata ayıklama | CORS, re-render döngüsü ve EF Core versiyon hataları yapay zeka yardımıyla çözüldü |
| Dokümantasyon | Bu rapor yapay zeka desteğiyle hazırlandı |
| Backend mimari | Controller → Manager → Repository katman yapısı bizzat tasarlandı; yapay zeka danışma amaçlı kullanıldı |

---

## Geliştirilebilecek Özellikler

**Kritik (production için zorunlu)**

| Özellik | Açıklama |
|---------|----------|
| Authentication & Authorization | JWT tabanlı login; CompanyId token claim'inden alınmalı, DTO'lardan çıkarılmalı |
| Kullanıcı yönetimi | IslemYapan alanı şu an elle yazılıyor, login olan kullanıcı otomatik gelmeli |
| Atomik transaction | Stok güncelleme ve hareket kaydı tek `IDbContextTransaction` içinde yapılmalı |
| Rol bazlı yetkilendirme | Depocu yalnızca giriş/çıkış yapabilmeli, ürün silememeli |

**v2 için**

| Özellik | Açıklama |
|---------|----------|
| Tarih filtreli raporlama | Dönem bazlı hareket özeti |
| Depolar arası transfer | Tek işlemde A → B deposuna taşıma |
| Excel / PDF export | Liste dışa aktarma |
| Kritik stok bildirimi | E-posta veya webhook ile otomatik uyarı |
| Hareket iptali | IptalEdildi / IptalEdenHareketId alanları hazır, iş mantığı eklenecek |

---

## Kurulum ve Çalıştırma

### Backend

```bash
cd backend/DepoYonetimi.API
dotnet restore
dotnet ef database update
dotnet run
```

`http://localhost:5143/swagger` adresinden Swagger UI'a erişilebilir.

### Frontend

```bash
cd frontend/depo-yonetimi
npm install
npm run dev
```

`http://localhost:5174` adresinden uygulamaya erişilebilir.

> İlk çalıştırmada seed data otomatik yüklenir: 6 ürün, 4 depo, 7 stok kaydı, 8 hareket.
