import api from './api';
import type {
  SayfalananYanit, Urun, UrunOlusturDto, UrunGuncelleDto, UrunSilDto,
} from '../tipler';

const BASE = '/urun';

export const urunServis = {
  listele: (companyId: string, arama?: string, kategori?: string, sayfa = 1, sayfaBoyutu = 25) =>
    api.get<SayfalananYanit<Urun>>(BASE + '/listele', {
      params: { companyId, Arama: arama, Kategori: kategori, Sayfa: sayfa, SayfaBoyutu: sayfaBoyutu },
    }),

  getById: (id: number, companyId: string) =>
    api.get<{ success: boolean; data: Urun }>(`${BASE}/${id}`, { params: { companyId } }),

  olustur: (dto: UrunOlusturDto) =>
    api.post<{ success: boolean; data: Urun; message: string }>(BASE + '/olustur', dto),

  guncelle: (dto: UrunGuncelleDto) =>
    api.post<{ success: boolean; data: Urun; message: string }>(BASE + '/guncelle', dto),

  sil: (dto: UrunSilDto) =>
    api.post<{ success: boolean; message: string }>(BASE + '/sil', dto),

  kategoriler: (companyId: string) =>
    api.get<{ success: boolean; data: string[] }>(BASE + '/kategoriler', { params: { companyId } }),
};
