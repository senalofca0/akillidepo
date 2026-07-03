import { Grid, Card, CardContent, Typography, Box } from '@mui/material';
import InventoryIcon from '@mui/icons-material/Inventory';
import WarehouseIcon from '@mui/icons-material/Warehouse';
import WarningAmberIcon from '@mui/icons-material/WarningAmber';
import CompareArrowsIcon from '@mui/icons-material/CompareArrows';
import type { DashboardOzet } from '../tipler';

interface Props {
  ozet: DashboardOzet | null;
  yukleniyor: boolean;
}

interface KartVeri {
  baslik: string;
  deger: number;
  renk: string;
  ikon: React.ReactNode;
  aciklama: string;
}

export default function OzetKartlar({ ozet, yukleniyor }: Props) {
  const kartlar: KartVeri[] = [
    {
      baslik: 'Toplam Ürün',
      deger: ozet?.toplamUrun ?? 0,
      renk: '#1976d2',
      ikon: <InventoryIcon sx={{ fontSize: 40, color: '#fff' }} />,
      aciklama: 'Tanımlı ürün sayısı',
    },
    {
      baslik: 'Aktif Depo',
      deger: ozet?.toplamDepo ?? 0,
      renk: '#388e3c',
      ikon: <WarehouseIcon sx={{ fontSize: 40, color: '#fff' }} />,
      aciklama: 'Aktif depo lokasyonu',
    },
    {
      baslik: 'Kritik Stok',
      deger: ozet?.kritikStokSayisi ?? 0,
      renk: ozet && ozet.kritikStokSayisi > 0 ? '#d32f2f' : '#f57c00',
      ikon: <WarningAmberIcon sx={{ fontSize: 40, color: '#fff' }} />,
      aciklama: 'Min. seviye altındaki ürünler',
    },
    {
      baslik: 'Toplam Hareket',
      deger: ozet?.toplamHareket ?? 0,
      renk: '#7b1fa2',
      ikon: <CompareArrowsIcon sx={{ fontSize: 40, color: '#fff' }} />,
      aciklama: 'Toplam giriş/çıkış',
    },
  ];

  return (
    <Grid container spacing={3} sx={{ mb: 3 }}>
      {kartlar.map((kart) => (
        <Grid item xs={12} sm={6} md={3} key={kart.baslik}>
          <Card elevation={3} sx={{ borderRadius: 3 }}>
            <CardContent sx={{ p: 0 }}>
              <Box sx={{ display: 'flex', alignItems: 'stretch' }}>
                <Box
                  sx={{
                    backgroundColor: kart.renk,
                    px: 2,
                    py: 2,
                    display: 'flex',
                    alignItems: 'center',
                    borderRadius: '12px 0 0 12px',
                    minWidth: 72,
                    justifyContent: 'center',
                  }}
                >
                  {kart.ikon}
                </Box>
                <Box sx={{ px: 2, py: 1.5, flex: 1 }}>
                  <Typography variant="body2" color="text.secondary" fontWeight={500}>
                    {kart.baslik}
                  </Typography>
                  <Typography variant="h4" fontWeight={700} color={kart.renk}>
                    {yukleniyor ? '–' : kart.deger.toLocaleString('tr-TR')}
                  </Typography>
                  <Typography variant="caption" color="text.disabled">
                    {kart.aciklama}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
  );
}
