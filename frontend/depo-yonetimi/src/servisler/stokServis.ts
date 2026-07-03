import api from './api';
import type { SayfalananYanit, Stok, DashboardOzet } from '../tipler';

const BASE = '/stok';

export const stokServis = {
  listele: (companyId: string, urunId?: number, depoId?: number, sayfa = 1, sayfaBoyutu = 25) =>
    api.get<SayfalananYanit<Stok>>(BASE + '/listele', {
      params: { companyId, urunId, depoId, sayfa, sayfaBoyutu },
    }),

  ozet: (companyId: string) =>
    api.get<{ success: boolean; data: DashboardOzet }>(BASE + '/ozet', { params: { companyId } }),
};
