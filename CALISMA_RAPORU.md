# Akıllı Depo Yönetimi — Çalışma Raporu

**Tarih:** 3 Temmuz 2026

---

## 📋 Ne Yapıldı?

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






---

## 
## 

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

## 🤖 Yapay Zeka Kullanımı

**Araç:** Chat Gpt

| Aşama | Yapay Zeka Kullanımı |
|---|---|
| TypeScript tipleri, servis katmanı |
|  | Port uyumsuzluğu, re-render sorunu, EF Core versiyon hatası |
| Seed Data | Örnek veri üretimi |
| Dokümantasyon |



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
