// ─── Sayfalama ───────────────────────────────────────────────────────────────
export interface SayfalananYanit<T> {
  success: boolean;
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  message?: string;
}

// ─── Ürün ────────────────────────────────────────────────────────────────────
export interface Urun {
  id: number;
  companyId: string;
  urunKodu: string;
  urunAdi: string;
  aciklama?: string;
  kategori: string;
  birim: string;
  birimFiyat?: number;
  barkod?: string;
  olusturmaTarihi: string;
  guncellemeTarihi?: string;
  toplamStok: number;
}

export interface UrunOlusturDto {
  CompanyId: string;
  UrunKodu: string;
  UrunAdi: string;
  Aciklama?: string;
  Kategori: string;
  Birim: string;
  BirimFiyat?: number;
  Barkod?: string;
}

export interface UrunGuncelleDto {
  Id: number;
  CompanyId: string;
  UrunAdi: string;
  Aciklama?: string;
  Kategori: string;
  Birim: string;
  BirimFiyat?: number;
  Barkod?: string;
}

export interface UrunSilDto {
  Id: number;
  CompanyId: string;
}

// ─── Depo ────────────────────────────────────────────────────────────────────
export interface Depo {
  id: number;
  companyId: string;
  depoKodu: string;
  depoAdi: string;
  konum?: string;
  bolge: string;
  raf: string;
  kapasite: number;
  aktifMi: boolean;
  toplamStok: number;
  urunCesidi: number;
  olusturmaTarihi: string;
}

export interface DepoOlusturDto {
  CompanyId: string;
  DepoKodu: string;
  DepoAdi: string;
  Konum?: string;
  Bolge: string;
  Raf: string;
  Kapasite: number;
  AktifMi: boolean;
}

export interface DepoGuncelleDto {
  Id: number;
  CompanyId: string;
  DepoAdi: string;
  Konum?: string;
  Bolge: string;
  Raf: string;
  Kapasite: number;
  AktifMi: boolean;
}

export interface DepoSilDto {
  Id: number;
  CompanyId: string;
}

// ─── Hareket ─────────────────────────────────────────────────────────────────
export interface DepoHareket {
  id: number;
  companyId: string;
  urunId: number;
  urunKodu: string;
  urunAdi: string;
  depoId: number;
  depoKodu: string;
  depoAdi: string;
  hareketTipi: 'Giris' | 'Cikis';
  miktar: number;
  hareketTarihi: string;
  aciklama?: string;
  referansNo?: string;
  islemYapan: string;
}

export interface HareketOlusturDto {
  CompanyId: string;
  UrunId: number;
  DepoId: number;
  HareketTipi: 'Giris' | 'Cikis';
  Miktar: number;
  HareketTarihi?: string;
  Aciklama?: string;
  ReferansNo?: string;
  IslemYapan: string;
}

// ─── Stok ─────────────────────────────────────────────────────────────────────
export interface Stok {
  id: number;
  companyId: string;
  urunId: number;
  urunKodu: string;
  urunAdi: string;
  kategori: string;
  depoId: number;
  depoKodu: string;
  depoAdi: string;
  bolge: string;
  raf: string;
  miktar: number;
  minimumStokSeviyesi: number;
  maksimumStokSeviyesi: number;
  kritikSeviyede: boolean;
}

// ─── Özet ─────────────────────────────────────────────────────────────────────
export interface DashboardOzet {
  toplamUrun: number;
  toplamDepo: number;
  kritikStokSayisi: number;
  toplamHareket: number;
}
