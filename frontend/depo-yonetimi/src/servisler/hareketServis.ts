import api from './api';
import type { SayfalananYanit, DepoHareket, HareketOlusturDto } from '../tipler';

const BASE = '/hareket';

export const hareketServis = {
  listele: (
    companyId: string,
    urunId?: number,
    depoId?: number,
    hareketTipi?: string,
    baslangicTarihi?: string,
    bitisTarihi?: string,
    sayfa = 1,
    sayfaBoyutu = 25,
  ) =>
    api.get<SayfalananYanit<DepoHareket>>(BASE + '/listele', {
      params: { companyId, UrunId: urunId, DepoId: depoId, HareketTipi: hareketTipi, BaslangicTarihi: baslangicTarihi, BitisTarihi: bitisTarihi, Sayfa: sayfa, SayfaBoyutu: sayfaBoyutu },
    }),

  isle: (dto: HareketOlusturDto) =>
    api.post<{ success: boolean; data: DepoHareket; message: string }>(BASE + '/isle', dto),
};
