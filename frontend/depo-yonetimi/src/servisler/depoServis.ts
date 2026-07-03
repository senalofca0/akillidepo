import api from './api';
import type {
  SayfalananYanit, Depo, DepoOlusturDto, DepoGuncelleDto, DepoSilDto,
} from '../tipler';

const BASE = '/depo';

export const depoServis = {
  listele: (companyId: string, arama?: string, sayfa = 1, sayfaBoyutu = 25) =>
    api.get<SayfalananYanit<Depo>>(BASE + '/listele', {
      params: { companyId, arama, sayfa, sayfaBoyutu },
    }),

  hepsi: (companyId: string) =>
    api.get<{ success: boolean; data: Depo[] }>(BASE + '/hepsi', { params: { companyId } }),

  olustur: (dto: DepoOlusturDto) =>
    api.post<{ success: boolean; data: Depo; message: string }>(BASE + '/olustur', dto),

  guncelle: (dto: DepoGuncelleDto) =>
    api.post<{ success: boolean; data: Depo; message: string }>(BASE + '/guncelle', dto),

  sil: (dto: DepoSilDto) =>
    api.post<{ success: boolean; message: string }>(BASE + '/sil', dto),
};
