# Akıllı Depo Yönetimi — Çalışma Raporu

**Tarih:** 3 Temmuz 2026

---

## � Proje Amacı

Çok şirketli (multi-tenant) depo, stok ve hareket takibini tek ekrandan yönetmek için geliştirilmiş full-stack bir web uygulamasıdır.

Farklı sektörlerdeki şirketlerin (üretim, eczane, lojistik vb.) depolarını ve ürün stoklarını merkezi olarak yönetmesini sağlar. Kullanıcılar ürün tanımları yapabilir, depolara stok atayabilir, giriş/çıkış hareketleri girebilir ve stok seviyelerini anlık olarak takip edebilir.

---

## �📋 Ne Yapıldı?

Akıllı Depo Yönetimi modülü, verilen teknik gereksinimler ve mimari kurallar doğrultusunda **sıfırdan** geliştirildi. Sistem; ürün tanımlama, depo yönetimi, stok takibi ve giriş/çıkış hareketlerini kapsayan tam özellikli bir full-stack uygulamadır.

**Tamamlanan başlıklar:**
- .NET 9.0 Backend API — Controller → Manager → Repository → Entity katmanlı mimari
- React 18 + TypeScript + Material-UI tek sayfa frontend
- EF Core Migration tabanlı MS SQL Server veritabanı şeması
- Tüm listeleme endpoint'lerinde server-side pagination (arama + filtre)
- Multi-tenant yapı: CompanyId bazlı tam veri izolasyonu
- Soft delete: IsDeleted ile mantıksal silme, tüm entity'lerde
- Dashboard özet kartları, sayfalanmış tablolar, ekleme/düzenleme/silme modalları
- Otomatik seed data ile çalışır halde teslim

---

## 🛠️ Kullanılan Teknolojiler ve Versiyonları

| Katman | Teknoloji | Versiyon |
|---|---|---|
| Backend | .NET / ASP.NET Core Web API | 9.0.200 |
| ORM | Entity Framework Core | 9.0.6 |
| Veritabanı | MS SQL Server | LocalDB / Full |
| API Dokümantasyon | Swashbuckle (Swagger) | 6.9.0 |
| Frontend | React | 18.2.0 |
| Dil | TypeScript | 4.6.4 |
| Build Tool | Vite | 3.2.3 |
| UI Kütüphanesi | Material-UI (MUI) | 5.14.0 |
| HTTP Client | Axios | 1.4.0 |
| Stil | Emotion | 11.11.0 |

---

## 🏗️ Mimari Kararlar ve Nedenleri

### 1. TemelVarlik (Base Entity) Soyut Sınıfı
Tüm entity'ler `TemelVarlik` sınıfından kalıtım alır. Bu sınıf `Id`, `CompanyId`, `IsDeleted`, `OlusturmaTarihi`, `GuncellemeTarihi` alanlarını merkezi olarak tanımlar. Sonuç: her entity'de tekrar kod yazmaya gerek kalmadı, multi-tenant ve soft-delete garantili oldu.

### 2. Stok Modeli: Anlık Durum + Hareket Geçmişi Ayrımı
`Stok` tablosu anlık miktarı tutar, `DepoHareket` tablosu tüm giriş/çıkış tarihçesini saklar. Bu yaklaşım iki avantaj sağlar: stok sorgulama hızlı (aggregate hesaplama yok), hareket geçmişi eksiksiz korunuyor.

### 3. Proje Klasör Yapısı

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

## 🗄️ Veritabanı Tablo Yapısı

### TemelVarlik (Tüm tablolara kalıtılan taban sınıf)

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

| Alan                 | Tip | Açıklama                     |
|----------------------|-----|------------------------------|
| UrunId               | int | FK → Urunler                 |
| DepoId               | int | FK → Depolar                 |
| Miktar               | int | Güncel stok miktarı          |
| MinimumStokSeviyesi  | int | İkaz seviyesi (minimum eşik) |
| MaksimumStokSeviyesi | int | Hedef maksimum seviye        |

> Her ürün/depo çifti için tek bir Stok kaydı tutulur. Hareket girildiğinde bu kayıt otomatik güncellenir.

### DepoHareketleri

| Alan               | Tip      | Açıklama                                   |
|--------------------|----------|--------------------------------------------|
| UrunId             | int      | FK → Urunler                               |
| DepoId             | int      | FK → Depolar                               |
| HareketTipi        | string   | `Giris` veya `Cikis`                       |
| Miktar             | int      | Hareket miktarı                            |
| HareketTarihi      | DateTime | İşlem tarihi                               |
| ReferansNo         | string?  | Sipariş no, fatura no vb.                  |
| IslemYapan         | string   | Hareketi giren kullanıcı adı               |
| IslemNo            | string   | Otomatik üretilen benzersiz işlem numarası |
| IptalEdildi        | bool     | Bu hareket iptal edildi mi?                |
| IptalEdenHareketId | int?     | İptal eden karşı hareketin Id'si           |
| IpAdresi           | string?  | İşlemi yapan kullanıcının IP adresi        |

---

## 🔌 API Endpoint'leri

| Controller | Prefix         | Temel İşlemler                         |
|------------|----------------|----------------------------------------|
| Urun       | `/api/urun`    | Ürün CRUD, listeleme, filtreleme       |
| Depo       | `/api/depo`    | Depo CRUD, kapasite sorgulama          |
| Stok       | `/api/stok`    | Stok sorgulama, dashboard özeti        |
| Hareket    | `/api/hareket` | Giriş/çıkış hareketi, hareket geçmişi |

Swagger UI: `http://localhost:5143/swagger`

---

## ✅ MVP Kapsamı ve Kapsam Dışı

**Teslim edilen özellikler:**
- Multi-tenant veri izolasyonu (CompanyId bazlı)
- Soft delete tüm entity'lerde
- Server-side pagination tüm listelerde
- Stok uyarıları (minimum seviye altı)

**Bilinçli olarak kapsam dışı bırakılanlar (production için gerekli):**
- JWT Authentication — production'da `CompanyId` token claim'inden alınmalı
- Hareket iptali iş mantığı — entity alanları hazır, endpoint henüz yok
- Atomik transaction — stok + hareket tek `IDbContextTransaction`'a alınmalı






---

## 👥 Bu Sistem Kim İçin Kullanılabilir?

Bu modül **multi-tenant** mimarisi sayesinde farklı sektörlere kolayca uyarlanabilir. Her müşteri kendi `CompanyId`'si ile tamamen izole çalışır.

### Kullanım Profilleri

**Üretim Firması / Fabrika**
- Hammadde, yarı mamul ve bitmiş ürün depoları ayrı ayrı yönetilir
- Vardiya bazlı giriş/çıkış kaydı
- Kritik stok uyarısı üretim durmadan önce müdahaleye olanak sağlar

**E-ticaret / Perakende**
- Birden fazla fiziksel depo veya şube tek ekranda
- Kargo çıkışı = stok düşümü
- Barkod alanı mevcut, okuyucu entegrasyonuna hazır

**Kurumsal / IT Stok Yönetimi**
- Laptop, ekipman, sarf malzeme takibi
- İşlemi yapan alanı ile kimin ne aldığı kayıt altında

---

## 🔭 Geliştirilebilecek Özellikler

Bu modül bir MVP (Minimum Viable Product) olarak teslim edilmektedir. Gerçek kullanım senaryolarında aşağıdaki geliştirmeler öncelikli ele alınmalıdır.

### 🔴 Kritik (Production için zorunlu)

| Özellik | Açıklama |
|---|---|
| **Authentication & Authorization** | JWT tabanlı login, kullanıcı rolleri (Admin, Depocu, Okuyucu). Şu an CompanyId sabitte kodlu |
| **Kullanıcı Yönetimi** | "IslemYapan" alanı elle yazılıyor; gerçekte login olan kullanıcı otomatik gelmeli |
| **Stok Düzeltme / Sayım** | Fiziksel sayım sonucu sisteme girilememekte. Fark kaydı oluşturulabilmeli |
| **Rol Bazlı Yetkilendirme** | Depocu yalnızca giriş/çıkış yapabilmeli, ürün silememeli |

### 🟡 Önemli (v2 için)

| Özellik | Açıklama |
|---|---|
| **Tarih Filtreli Raporlama** | "Bu ay kaç çıkış yapıldı?" sorusu cevaplanamamakta |
| **Depolar Arası Transfer** | A → B deposuna taşıma için şu an 2 ayrı işlem gerekiyor |
| **Excel / PDF Export** | Stok ve hareket listelerini dışa aktarma |
| **Kritik Stok Bildirimi** | E-posta veya webhook ile otomatik uyarı |
| **Hareket Sebebi / Kategorisi** | Çıkış = Satış mı, Fire mi, İade mi? Ayrıştırılabilmeli |

### 🟢 Nice-to-have (v3 için)

| Özellik | Açıklama |
|---|---|
| Barkod Okuyucu Entegrasyonu | USB veya kamera ile hızlı işlem |
| Tedarikçi Yönetimi | Stok girişi hangi tedarikçiden geldi |
| FIFO / LIFO Takibi | İlk giren ilk çıkar lojistiği |
| Mobil Uyumlu Arayüz | Depo personeli tablet/telefon kullanabilmeli |
| Çoklu Dil Desteği | i18n altyapısı |

---

## ⚠️ Karşılaşılan Sorunlar ve Çözüm Yolları

| Sorun | Çözüm |
|---|---|
| EF Core migration sonrası `RowVersion` alanı SQL Server'da `rowversion` tipine dönüşmedi | `[Timestamp]` attribute'u `TemelVarlik`'e eklendi, migration yeniden oluşturuldu |
| Frontend'den backend'e istek atılırken CORS hatası | `Program.cs`'e `AddCors` + `UseCors` politikası eklendi, `localhost:5174` izin listesine alındı |
| Stok güncelleme sırasında `EntityState` sorunu — değişiklikler kaydedilmiyordu | Tüm `GuncelleAsync` metodlarına `_context.Entry(entity).State = EntityState.Modified` eklendi |
| Frontend'de büyük veri setlerinde sayfa yavaşlıyordu | Client-side filtreleme kaldırıldı, tüm listeleme server-side pagination'a alındı |

---

## 🤖 Yapay Zeka Kullanımı

**Araç:** ChatGPT / Kiro (AI destekli IDE)

| Aşama | Kullanım Detayı |
|---|---|
| **Frontend geliştirme** | Önyüz bileşenleri (tablolar, modallar, form yapıları) geliştirilirken yapay zekadan yararlanıldı. MUI bileşen yapıları, TypeScript tip tanımları ve servis katmanı kodları yapay zeka desteğiyle oluşturuldu |
| **Seed Data** | Gerçekçi örnek veri üretimi için yapay zeka kullanıldı |
| **Hata ayıklama** | Port uyumsuzluğu, re-render döngüsü ve EF Core versiyon hataları yapay zeka yardımıyla çözüldü |
| **Dokümantasyon** | Bu rapor yapay zeka desteğiyle hazırlandı |
| **Backend mimari** | Controller → Manager → Repository katman yapısı bizzat tasarlandı; yapay zeka yalnızca danışma amaçlı kullanıldı |



---

## 🚀 Kurulum ve Çalıştırma

### Backend
```bash
cd backend/DepoYonetimi.API
dotnet restore
dotnet ef database update     # Veritabanı + tablolar oluşturulur
dotnet run                    # http://localhost:5143
```
Swagger: `http://localhost:5143/swagger`

### Frontend
```bash
cd frontend/depo-yonetimi
npm install
npm run dev                   # http://localhost:5174
```

> İlk çalıştırmada seed data otomatik yüklenir (6 ürün, 4 depo, 7 stok kaydı, 8 hareket).

---

*Bu rapor, geliştirme sürecinin tamamını ve sistemin ileriye dönük potansiyelini kapsamaktadır.*
