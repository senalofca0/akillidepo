import { useState, useEffect } from 'react';
import {
  Box, Tabs, Tab, Button, Paper, Snackbar, Alert, Typography,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TablePagination,
  IconButton, Chip, Dialog, DialogTitle, DialogContent, DialogActions, TextField,
  MenuItem, CircularProgress, Select, FormControl, InputLabel, Grid, Container,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SwapVertIcon from '@mui/icons-material/SwapVert';
import SearchIcon from '@mui/icons-material/Search';
import OzetKartlar from '../bilesenler/OzetKartlar';
import { stokServis } from '../servisler/stokServis';
import { urunServis } from '../servisler/urunServis';
import { depoServis } from '../servisler/depoServis';
import { hareketServis } from '../servisler/hareketServis';
import type {
  DashboardOzet, Urun, Depo, DepoHareket, Stok,
} from '../tipler';

const companyId = 'DEMO-001';

const SIRKETLER = [
  { id: 'DEMO-001', ad: 'ABC Üretim A.Ş.', ikon: '🏭' },
  { id: 'DEMO-002', ad: 'Medi-Raf Eczane Zinciri', ikon: '💊' },
];

// Sabit form tipleri
interface UrunForm { UrunKodu: string; UrunAdi: string; Aciklama: string; Kategori: string; YeniKategori: string; Birim: string; BirimFiyat: string; Barkod: string; IlkDepoId: string; IlkMiktar: string; }
interface DepoForm { DepoKodu: string; DepoAdi: string; Konum: string; Bolge: string; Raf: string; Kapasite: string; AktifMi: boolean; }
interface HareketForm { UrunId: string; DepoId: string; HareketTipi: string; Miktar: string; Aciklama: string; ReferansNo: string; IslemYapan: string; }

const bUrun: UrunForm = { UrunKodu: '', UrunAdi: '', Aciklama: '', Kategori: '', YeniKategori: '', Birim: 'Adet', BirimFiyat: '', Barkod: '', IlkDepoId: '', IlkMiktar: '' };
const bDepo: DepoForm = { DepoKodu: '', DepoAdi: '', Konum: '', Bolge: '', Raf: '', Kapasite: '1000', AktifMi: true };
const bHareket: HareketForm = { UrunId: '', DepoId: '', HareketTipi: 'Giris', Miktar: '1', Aciklama: '', ReferansNo: '', IslemYapan: 'Kullanıcı' };

export default function DepoYonetimiSayfasi() {
  const [sekme, setSekme] = useState(0);
  const [companyId, setCompanyId] = useState('DEMO-001');
  const [ozet, setOzet] = useState<DashboardOzet | null>(null);
  const [yukl, setYukl] = useState(false);
  const [bildirim, setBildirim] = useState<{ m: string; t: 'success' | 'error' | 'info' | 'warning' } | null>(null);

  // Tablo state
  const [urunler, setUrunler] = useState<Urun[]>([]);
  const [urunSayfa, setUrunSayfa] = useState(0);
  const [urunBoyut, setUrunBoyut] = useState(25);
  const [urunToplam, setUrunToplam] = useState(0);
  const [urunArama, setUrunArama] = useState('');
  const [urunKat, setUrunKat] = useState('');
  const [kategoriler, setKategoriler] = useState<string[]>([]);

  const [depolar, setDepolar] = useState<Depo[]>([]);
  const [depoSayfa, setDepoSayfa] = useState(0);
  const [depoBoyut, setDepoBoyut] = useState(25);
  const [depoToplam, setDepoToplam] = useState(0);
  const [depoArama, setDepoArama] = useState('');

  const [hareketler, setHareketler] = useState<DepoHareket[]>([]);
  const [hareketSayfa, setHareketSayfa] = useState(0);
  const [hareketToplam, setHareketToplam] = useState(0);
  const [hareketTip, setHareketTip] = useState('');

  const [stoklar, setStoklar] = useState<Stok[]>([]);
  const [stokSayfa, setStokSayfa] = useState(0);
  const [stokToplam, setStokToplam] = useState(0);

  // Modal state
  const [modal, setModal] = useState<'urun-ekle' | 'urun-duzenle' | 'depo-ekle' | 'depo-duzenle' | 'hareket' | null>(null);
  const [silAcik, setSilAcik] = useState(false);
  const [silId, setSilId] = useState<number | null>(null);
  const [silTur, setSilTur] = useState<'urun' | 'depo'>('urun');
  const [kaydYukl, setKaydYukl] = useState(false);
  const [seciliId, setSeciliId] = useState<number | null>(null);

  // ÖNEMLİ: form state'leri üst seviyede — iç bileşen değil
  const [uF, setUF] = useState<UrunForm>(bUrun);
  const [dF, setDF] = useState<DepoForm>(bDepo);
  const [hF, setHF] = useState<HareketForm>(bHareket);

  const [hUrunler, setHUrunler] = useState<Urun[]>([]);
  const [hDepolar, setHDepolar] = useState<Depo[]>([]);

  const bildir = (m: string, t: 'success' | 'error') => setBildirim({ m, t });

  // ─── Veri yükleme ─────────────────────────────────────────────────────────
  const ozetYukle = async () => {
    try { const r = await stokServis.ozet(companyId); setOzet(r.data.data); } catch { }
  };

  const urunYukle = async () => {
    setYukl(true);
    try {
      const r = await urunServis.listele(companyId, urunArama, urunKat, urunSayfa + 1, urunBoyut);
      setUrunler(r.data.data); setUrunToplam(r.data.totalCount);
    } catch { bildir('Ürünler yüklenemedi', 'error'); }
    finally { setYukl(false); }
  };

  const depoYukle = async () => {
    setYukl(true);
    try {
      const r = await depoServis.listele(companyId, depoArama, depoSayfa + 1, depoBoyut);
      setDepolar(r.data.data); setDepoToplam(r.data.totalCount);
    } catch { bildir('Depolar yüklenemedi', 'error'); }
    finally { setYukl(false); }
  };

  const hareketYukle = async () => {
    setYukl(true);
    try {
      const r = await hareketServis.listele(companyId, undefined, undefined, hareketTip || undefined, undefined, undefined, hareketSayfa + 1, 25);
      setHareketler(r.data.data); setHareketToplam(r.data.totalCount);
    } catch { bildir('Hareketler yüklenemedi', 'error'); }
    finally { setYukl(false); }
  };

  const stokYukle = async () => {
    setYukl(true);
    try {
      const r = await stokServis.listele(companyId, undefined, undefined, stokSayfa + 1, 25);
      setStoklar(r.data.data); setStokToplam(r.data.totalCount);
    } catch { bildir('Stoklar yüklenemedi', 'error'); }
    finally { setYukl(false); }
  };

  useEffect(() => { ozetYukle(); urunServis.kategoriler(companyId).then(r => setKategoriler(r.data.data)).catch(() => {}); }, [companyId]);
  useEffect(() => { if (sekme === 0) urunYukle(); }, [sekme, companyId, urunSayfa, urunBoyut, urunArama, urunKat]);
  useEffect(() => { if (sekme === 1) depoYukle(); }, [sekme, companyId, depoSayfa, depoBoyut, depoArama]);
  useEffect(() => { if (sekme === 2) stokYukle(); }, [sekme, companyId, stokSayfa]);
  useEffect(() => { if (sekme === 3) hareketYukle(); }, [sekme, companyId, hareketSayfa, hareketTip]);

  useEffect(() => {
    if (modal !== 'hareket' && modal !== 'urun-ekle') return;
    urunServis.listele(companyId, '', '', 1, 999).then(r => setHUrunler(r.data.data)).catch(() => {});
    depoServis.hepsi(companyId).then(r => setHDepolar(r.data.data)).catch(() => {});
  }, [modal]);

  // ─── Modal aç/kapat ───────────────────────────────────────────────────────
  const modalAc = (tip: typeof modal, u?: Urun, d?: Depo) => {
    if (tip === 'urun-ekle') setUF(bUrun);
    if (tip === 'urun-duzenle' && u) { setSeciliId(u.id); setUF({ UrunKodu: u.urunKodu, UrunAdi: u.urunAdi, Aciklama: u.aciklama || '', Kategori: u.kategori, YeniKategori: '', Birim: u.birim, BirimFiyat: u.birimFiyat?.toString() || '', Barkod: u.barkod || '', IlkDepoId: '', IlkMiktar: '' }); }
    if (tip === 'depo-ekle') setDF(bDepo);
    if (tip === 'depo-duzenle' && d) { setSeciliId(d.id); setDF({ DepoKodu: d.depoKodu, DepoAdi: d.depoAdi, Konum: d.konum || '', Bolge: d.bolge, Raf: d.raf, Kapasite: d.kapasite.toString(), AktifMi: d.aktifMi }); }
    if (tip === 'hareket') setHF(bHareket);
    setModal(tip);
  };
  const modalKapat = () => { setModal(null); setSeciliId(null); };

  // ─── Kaydet işlemleri ─────────────────────────────────────────────────────
  const urunKaydet = async () => {
    setKaydYukl(true);
    try {
      // Kategori: dropdown seçili yoksa YeniKategori alanını kullan
      const kategori = uF.Kategori !== '' ? uF.Kategori : uF.YeniKategori;
      if (!kategori) { bildir('Kategori zorunludur', 'error'); setKaydYukl(false); return; }

      if (modal === 'urun-ekle') {
        const r = await urunServis.olustur({ CompanyId: companyId, UrunKodu: uF.UrunKodu, UrunAdi: uF.UrunAdi, Aciklama: uF.Aciklama, Kategori: kategori, Birim: uF.Birim, BirimFiyat: uF.BirimFiyat ? Number(uF.BirimFiyat) : undefined, Barkod: uF.Barkod });
        // İlk stok girişi seçildiyse otomatik yap
        if (uF.IlkDepoId && uF.IlkMiktar && Number(uF.IlkMiktar) > 0) {
          await hareketServis.isle({ CompanyId: companyId, UrunId: r.data.data.id, DepoId: Number(uF.IlkDepoId), HareketTipi: 'Giris', Miktar: Number(uF.IlkMiktar), IslemYapan: 'Kullanıcı' });
          bildir('Ürün oluşturuldu ve stok girişi yapıldı', 'success');
        } else {
          bildir('Ürün oluşturuldu', 'success');
        }
      } else if (seciliId) {
        await urunServis.guncelle({ Id: seciliId, CompanyId: companyId, UrunAdi: uF.UrunAdi, Aciklama: uF.Aciklama, Kategori: kategori, Birim: uF.Birim, BirimFiyat: uF.BirimFiyat ? Number(uF.BirimFiyat) : undefined, Barkod: uF.Barkod });
        bildir('Ürün güncellendi', 'success');
      }
      modalKapat(); urunYukle(); ozetYukle();
      urunServis.kategoriler(companyId).then(r => setKategoriler(r.data.data)).catch(() => {});
    } catch (e: any) { bildir(e?.response?.data?.message || 'İşlem başarısız', 'error'); }
    finally { setKaydYukl(false); }
  };

  const depoKaydet = async () => {
    setKaydYukl(true);
    try {
      if (modal === 'depo-ekle') {
        await depoServis.olustur({ CompanyId: companyId, DepoKodu: dF.DepoKodu, DepoAdi: dF.DepoAdi, Konum: dF.Konum, Bolge: dF.Bolge, Raf: dF.Raf, Kapasite: Number(dF.Kapasite), AktifMi: dF.AktifMi });
        bildir('Depo oluşturuldu', 'success');
      } else if (seciliId) {
        await depoServis.guncelle({ Id: seciliId, CompanyId: companyId, DepoAdi: dF.DepoAdi, Konum: dF.Konum, Bolge: dF.Bolge, Raf: dF.Raf, Kapasite: Number(dF.Kapasite), AktifMi: dF.AktifMi });
        bildir('Depo güncellendi', 'success');
      }
      modalKapat(); depoYukle(); ozetYukle();
    } catch (e: any) { bildir(e?.response?.data?.message || 'İşlem başarısız', 'error'); }
    finally { setKaydYukl(false); }
  };

  const hareketKaydet = async () => {
    setKaydYukl(true);
    try {
      await hareketServis.isle({ CompanyId: companyId, UrunId: Number(hF.UrunId), DepoId: Number(hF.DepoId), HareketTipi: hF.HareketTipi as 'Giris' | 'Cikis', Miktar: Number(hF.Miktar), Aciklama: hF.Aciklama, ReferansNo: hF.ReferansNo, IslemYapan: hF.IslemYapan });
      bildir(hF.HareketTipi === 'Giris' ? 'Ürün depoya girildi' : 'Ürün depodan çıkarıldı', 'success');
      modalKapat(); hareketYukle(); stokYukle(); ozetYukle();
    } catch (e: any) { bildir(e?.response?.data?.message || 'İşlem başarısız', 'error'); }
    finally { setKaydYukl(false); }
  };

  const silOnayla = async () => {
    if (!silId) return;
    setKaydYukl(true);
    try {
      if (silTur === 'urun') { await urunServis.sil({ Id: silId, CompanyId: companyId }); bildir('Ürün silindi', 'success'); urunYukle(); }
      else { await depoServis.sil({ Id: silId, CompanyId: companyId }); bildir('Depo silindi', 'success'); depoYukle(); }
      ozetYukle();
    } catch (e: any) { bildir(e?.response?.data?.message || 'Silme başarısız', 'error'); }
    finally { setKaydYukl(false); setSilAcik(false); setSilId(null); }
  };

  return (
    <Box sx={{ bgcolor: 'background.default', minHeight: '100vh' }}>

      {/* Header */}
      <Box sx={{ bgcolor: '#1976d2', color: '#fff', px: 4, py: 2, boxShadow: 3 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', maxWidth: 1400, mx: 'auto' }}>
          <Typography variant="h5" fontWeight={800} letterSpacing={1}>📦 DEPO YÖNETİMİ</Typography>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <Typography variant="body2" sx={{ color: 'rgba(255,255,255,0.8)' }}>Şirket:</Typography>
            <Select
              value={companyId}
              onChange={e => {
                setCompanyId(e.target.value);
                setSekme(0);
                setUrunSayfa(0); setDepoSayfa(0); setStokSayfa(0); setHareketSayfa(0);
                setUrunArama(''); setDepoArama(''); setUrunKat(''); setHareketTip('');
              }}
              size="small"
              sx={{
                bgcolor: 'rgba(255,255,255,0.15)',
                color: '#fff',
                fontWeight: 700,
                minWidth: 220,
                '.MuiOutlinedInput-notchedOutline': { borderColor: 'rgba(255,255,255,0.4)' },
                '&:hover .MuiOutlinedInput-notchedOutline': { borderColor: '#fff' },
                '.MuiSvgIcon-root': { color: '#fff' },
              }}
            >
              {SIRKETLER.map(s => (
                <MenuItem key={s.id} value={s.id}>
                  {s.ikon} {s.ad}
                </MenuItem>
              ))}
            </Select>
          </Box>
        </Box>
      </Box>

      <Container maxWidth="xl" sx={{ py: 3 }}>
        <OzetKartlar ozet={ozet} yukleniyor={yukl} />

        <Paper elevation={2} sx={{ borderRadius: 3, overflow: 'hidden' }}>
          <Tabs value={sekme} onChange={(_, v) => setSekme(v)} variant="scrollable" scrollButtons="auto"
            sx={{ borderBottom: 1, borderColor: 'divider', bgcolor: 'grey.50', px: 2 }}>
            <Tab label="📦 Ürünler" />
            <Tab label="🏭 Depolar" />
            <Tab label="📊 Stok Durumu" />
            <Tab label="🔄 Hareketler" />
          </Tabs>

          <Box sx={{ p: 3 }}>

            {/* ─── ÜRÜNLER SEKMESİ ─── */}
            {sekme === 0 && (
              <Box>
                <Box sx={{ display: 'flex', gap: 2, mb: 2, flexWrap: 'wrap' }}>
                  <TextField size="small" placeholder="Ürün ara…" value={urunArama}
                    onChange={e => { setUrunArama(e.target.value); setUrunSayfa(0); }}
                    InputProps={{ startAdornment: <SearchIcon sx={{ mr: 0.5, color: 'text.disabled' }} /> }}
                    sx={{ minWidth: 220 }} />
                  <FormControl size="small" sx={{ minWidth: 160 }}>
                    <InputLabel>Kategori</InputLabel>
                    <Select label="Kategori" value={urunKat} onChange={e => { setUrunKat(e.target.value); setUrunSayfa(0); }}>
                      <MenuItem value="">Tümü</MenuItem>
                      {kategoriler.map(k => <MenuItem key={k} value={k}>{k}</MenuItem>)}
                    </Select>
                  </FormControl>
                  <Box sx={{ flex: 1 }} />
                  <Button variant="contained" startIcon={<AddIcon />} onClick={() => modalAc('urun-ekle')}>Yeni Ürün</Button>
                  <Button variant="outlined" color="secondary" startIcon={<SwapVertIcon />} onClick={() => modalAc('hareket')}>Giriş / Çıkış</Button>
                </Box>
                <TableContainer>
                  <Table size="small">
                    <TableHead sx={{ bgcolor: 'grey.100' }}>
                      <TableRow>
                        {['Ürün Kodu', 'Ürün Adı', 'Kategori', 'Birim', 'Birim Fiyat', 'Toplam Stok', 'İşlem'].map(h =>
                          <TableCell key={h} sx={{ fontWeight: 700 }}>{h}</TableCell>)}
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {yukl ? <TableRow><TableCell colSpan={7} align="center"><CircularProgress size={28} sx={{ my: 2 }} /></TableCell></TableRow>
                        : urunler.length === 0 ? <TableRow><TableCell colSpan={7} align="center" sx={{ py: 4, color: 'text.secondary' }}>Kayıt bulunamadı</TableCell></TableRow>
                          : urunler.map(u => (
                            <TableRow key={u.id} hover>
                              <TableCell><Chip label={u.urunKodu} size="small" variant="outlined" /></TableCell>
                              <TableCell>{u.urunAdi}</TableCell>
                              <TableCell><Chip label={u.kategori} size="small" color="primary" variant="outlined" /></TableCell>
                              <TableCell>{u.birim}</TableCell>
                              <TableCell>{u.birimFiyat != null ? `₺${Number(u.birimFiyat).toFixed(2)}` : '—'}</TableCell>
                              <TableCell><Chip label={u.toplamStok} size="small" color={u.toplamStok === 0 ? 'error' : u.toplamStok < 10 ? 'warning' : 'success'} /></TableCell>
                              <TableCell>
                                <IconButton size="small" color="primary" onClick={() => modalAc('urun-duzenle', u)}><EditIcon fontSize="small" /></IconButton>
                                <IconButton size="small" color="error" onClick={() => { setSilId(u.id); setSilTur('urun'); setSilAcik(true); }}><DeleteIcon fontSize="small" /></IconButton>
                              </TableCell>
                            </TableRow>
                          ))}
                    </TableBody>
                  </Table>
                </TableContainer>
                <TablePagination component="div" count={urunToplam} page={urunSayfa} rowsPerPage={urunBoyut}
                  rowsPerPageOptions={[10, 25, 50]} onPageChange={(_, p) => setUrunSayfa(p)}
                  onRowsPerPageChange={e => { setUrunBoyut(parseInt(e.target.value)); setUrunSayfa(0); }}
                  labelRowsPerPage="Sayfa başına:" labelDisplayedRows={({ from, to, count }) => `${from}–${to} / ${count}`} />
              </Box>
            )}

            {/* ─── DEPOLAR SEKMESİ ─── */}
            {sekme === 1 && (
              <Box>
                <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
                  <TextField size="small" placeholder="Depo ara…" value={depoArama}
                    onChange={e => { setDepoArama(e.target.value); setDepoSayfa(0); }}
                    InputProps={{ startAdornment: <SearchIcon sx={{ mr: 0.5, color: 'text.disabled' }} /> }} sx={{ minWidth: 220 }} />
                  <Box sx={{ flex: 1 }} />
                  <Button variant="contained" startIcon={<AddIcon />} onClick={() => modalAc('depo-ekle')}>Yeni Depo</Button>
                </Box>
                <TableContainer>
                  <Table size="small">
                    <TableHead sx={{ bgcolor: 'grey.100' }}>
                      <TableRow>
                        {['Depo Kodu', 'Depo Adı', 'Bölge / Raf', 'Konum', 'Kapasite', 'Ürün Çeşidi', 'Toplam Stok', 'Durum', 'İşlem'].map(h =>
                          <TableCell key={h} sx={{ fontWeight: 700 }}>{h}</TableCell>)}
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {yukl ? <TableRow><TableCell colSpan={9} align="center"><CircularProgress size={28} sx={{ my: 2 }} /></TableCell></TableRow>
                        : depolar.length === 0 ? <TableRow><TableCell colSpan={9} align="center" sx={{ py: 4, color: 'text.secondary' }}>Depo bulunamadı</TableCell></TableRow>
                          : depolar.map(d => (
                            <TableRow key={d.id} hover>
                              <TableCell><Chip label={d.depoKodu} size="small" variant="outlined" /></TableCell>
                              <TableCell>{d.depoAdi}</TableCell>
                              <TableCell>{d.bolge} / {d.raf}</TableCell>
                              <TableCell>{d.konum || '—'}</TableCell>
                              <TableCell>{d.kapasite}</TableCell>
                              <TableCell>{d.urunCesidi}</TableCell>
                              <TableCell><Chip label={d.toplamStok} size="small" color={d.toplamStok > 0 ? 'info' : 'default'} /></TableCell>
                              <TableCell><Chip label={d.aktifMi ? 'Aktif' : 'Pasif'} size="small" color={d.aktifMi ? 'success' : 'default'} /></TableCell>
                              <TableCell>
                                <IconButton size="small" color="primary" onClick={() => modalAc('depo-duzenle', undefined, d)}><EditIcon fontSize="small" /></IconButton>
                                <IconButton size="small" color="error" onClick={() => { setSilId(d.id); setSilTur('depo'); setSilAcik(true); }}><DeleteIcon fontSize="small" /></IconButton>
                              </TableCell>
                            </TableRow>
                          ))}
                    </TableBody>
                  </Table>
                </TableContainer>
                <TablePagination component="div" count={depoToplam} page={depoSayfa} rowsPerPage={depoBoyut}
                  rowsPerPageOptions={[10, 25, 50]} onPageChange={(_, p) => setDepoSayfa(p)}
                  onRowsPerPageChange={e => { setDepoBoyut(parseInt(e.target.value)); setDepoSayfa(0); }}
                  labelRowsPerPage="Sayfa başına:" labelDisplayedRows={({ from, to, count }) => `${from}–${to} / ${count}`} />
              </Box>
            )}

            {/* ─── STOK SEKMESİ ─── */}
            {sekme === 2 && (
              <Box>
                <TableContainer>
                  <Table size="small">
                    <TableHead sx={{ bgcolor: 'grey.100' }}>
                      <TableRow>
                        {['Ürün Kodu', 'Ürün Adı', 'Kategori', 'Depo', 'Bölge/Raf', 'Miktar', 'Min. Seviye', 'Durum'].map(h =>
                          <TableCell key={h} sx={{ fontWeight: 700 }}>{h}</TableCell>)}
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {yukl ? <TableRow><TableCell colSpan={8} align="center"><CircularProgress size={28} sx={{ my: 2 }} /></TableCell></TableRow>
                        : stoklar.length === 0 ? <TableRow><TableCell colSpan={8} align="center" sx={{ py: 4, color: 'text.secondary' }}>Stok kaydı yok</TableCell></TableRow>
                          : stoklar.map(s => (
                            <TableRow key={s.id} hover sx={{ bgcolor: s.kritikSeviyede ? '#fff3e0' : 'inherit' }}>
                              <TableCell><Chip label={s.urunKodu} size="small" variant="outlined" /></TableCell>
                              <TableCell>{s.urunAdi}</TableCell>
                              <TableCell>{s.kategori}</TableCell>
                              <TableCell>{s.depoAdi}</TableCell>
                              <TableCell>{s.bolge} / {s.raf}</TableCell>
                              <TableCell sx={{ fontWeight: 700 }}>{s.miktar}</TableCell>
                              <TableCell>{s.minimumStokSeviyesi}</TableCell>
                              <TableCell><Chip size="small" label={s.kritikSeviyede ? 'KRİTİK' : 'Normal'} color={s.kritikSeviyede ? 'error' : 'success'} /></TableCell>
                            </TableRow>
                          ))}
                    </TableBody>
                  </Table>
                </TableContainer>
                <TablePagination component="div" count={stokToplam} page={stokSayfa} rowsPerPage={25}
                  rowsPerPageOptions={[25]} onPageChange={(_, p) => setStokSayfa(p)}
                  labelRowsPerPage="" labelDisplayedRows={({ from, to, count }) => `${from}–${to} / ${count}`} />
              </Box>
            )}

            {/* ─── HAREKETLER SEKMESİ ─── */}
            {sekme === 3 && (
              <Box>
                <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
                  <FormControl size="small" sx={{ minWidth: 160 }}>
                    <InputLabel>Hareket Tipi</InputLabel>
                    <Select label="Hareket Tipi" value={hareketTip}
                      onChange={e => { setHareketTip(e.target.value); setHareketSayfa(0); }}>
                      <MenuItem value="">Tümü</MenuItem>
                      <MenuItem value="Giris">Giriş</MenuItem>
                      <MenuItem value="Cikis">Çıkış</MenuItem>
                    </Select>
                  </FormControl>
                  <Box sx={{ flex: 1 }} />
                  <Button variant="contained" color="secondary" startIcon={<SwapVertIcon />} onClick={() => modalAc('hareket')}>Giriş / Çıkış Yap</Button>
                </Box>
                <TableContainer>
                  <Table size="small">
                    <TableHead sx={{ bgcolor: 'grey.100' }}>
                      <TableRow>
                        {['Tarih', 'Tip', 'Ürün', 'Depo', 'Miktar', 'Referans No', 'İşlemi Yapan'].map(h =>
                          <TableCell key={h} sx={{ fontWeight: 700 }}>{h}</TableCell>)}
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {yukl ? <TableRow><TableCell colSpan={7} align="center"><CircularProgress size={28} sx={{ my: 2 }} /></TableCell></TableRow>
                        : hareketler.length === 0 ? <TableRow><TableCell colSpan={7} align="center" sx={{ py: 4, color: 'text.secondary' }}>Hareket kaydı yok</TableCell></TableRow>
                          : hareketler.map(h => (
                            <TableRow key={h.id} hover>
                              <TableCell>{new Date(h.hareketTarihi).toLocaleString('tr-TR')}</TableCell>
                              <TableCell><Chip size="small" label={h.hareketTipi === 'Giris' ? '▲ Giriş' : '▼ Çıkış'} color={h.hareketTipi === 'Giris' ? 'success' : 'error'} /></TableCell>
                              <TableCell><b>{h.urunKodu}</b> — {h.urunAdi}</TableCell>
                              <TableCell>{h.depoAdi}</TableCell>
                              <TableCell sx={{ fontWeight: 700 }}>{h.miktar}</TableCell>
                              <TableCell>{h.referansNo || '—'}</TableCell>
                              <TableCell>{h.islemYapan}</TableCell>
                            </TableRow>
                          ))}
                    </TableBody>
                  </Table>
                </TableContainer>
                <TablePagination component="div" count={hareketToplam} page={hareketSayfa} rowsPerPage={25}
                  rowsPerPageOptions={[25]} onPageChange={(_, p) => setHareketSayfa(p)}
                  labelRowsPerPage="" labelDisplayedRows={({ from, to, count }) => `${from}–${to} / ${count}`} />
              </Box>
            )}

          </Box>
        </Paper>
      </Container>

      {/* ─── ÜRÜN MODALI ─── */}
      <Dialog open={modal === 'urun-ekle' || modal === 'urun-duzenle'} onClose={modalKapat} maxWidth="sm" fullWidth>
        <DialogTitle sx={{ fontWeight: 700 }}>{modal === 'urun-ekle' ? 'Yeni Ürün Ekle' : 'Ürün Düzenle'}</DialogTitle>
        <DialogContent dividers><Box sx={{ pt: 1 }}>
          {modal === 'urun-ekle' && <TextField label="Ürün Kodu *" required fullWidth size="small" value={uF.UrunKodu} onChange={e => setUF(p => ({ ...p, UrunKodu: e.target.value }))} sx={{ mb: 2 }} />}
          <TextField label="Ürün Adı *" required fullWidth size="small" value={uF.UrunAdi} onChange={e => setUF(p => ({ ...p, UrunAdi: e.target.value }))} sx={{ mb: 2 }} />
          
          <Grid container spacing={2}>
            <Grid item xs={6}>
              <FormControl fullWidth size="small">
                <InputLabel>Kategori *</InputLabel>
                <Select label="Kategori *" value={uF.Kategori} onChange={e => setUF(p => ({ ...p, Kategori: e.target.value, YeniKategori: '' }))}>
                  <MenuItem value=""><em>--- Yeni Kategori ---</em></MenuItem>
                  {kategoriler.map(k => <MenuItem key={k} value={k}>{k}</MenuItem>)}
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={6}>
              {uF.Kategori === '' ? (
                <TextField label="Yeni Kategori Adı *" required fullWidth size="small" value={uF.YeniKategori} onChange={e => setUF(p => ({ ...p, YeniKategori: e.target.value }))} />
              ) : (
                <FormControl fullWidth size="small"><InputLabel>Birim</InputLabel>
                  <Select label="Birim" value={uF.Birim} onChange={e => setUF(p => ({ ...p, Birim: e.target.value }))}>
                    {['Adet', 'Kg', 'Litre', 'Metre', 'Kutu', 'Paket', 'Ton'].map(b => <MenuItem key={b} value={b}>{b}</MenuItem>)}
                  </Select>
                </FormControl>
              )}
            </Grid>
          </Grid>

          {uF.Kategori === '' && (
            <FormControl fullWidth size="small" sx={{ mt: 2 }}><InputLabel>Birim</InputLabel>
              <Select label="Birim" value={uF.Birim} onChange={e => setUF(p => ({ ...p, Birim: e.target.value }))}>
                {['Adet', 'Kg', 'Litre', 'Metre', 'Kutu', 'Paket', 'Ton'].map(b => <MenuItem key={b} value={b}>{b}</MenuItem>)}
              </Select>
            </FormControl>
          )}

          <Grid container spacing={2} sx={{ mt: 0 }}>
            <Grid item xs={6}><TextField label="Birim Fiyat (₺)" type="number" fullWidth size="small" value={uF.BirimFiyat} onChange={e => setUF(p => ({ ...p, BirimFiyat: e.target.value }))} /></Grid>
            <Grid item xs={6}><TextField label="Barkod (opsiyonel)" fullWidth size="small" value={uF.Barkod} onChange={e => setUF(p => ({ ...p, Barkod: e.target.value }))} helperText="Boş bırakılabilir" /></Grid>
          </Grid>
          <TextField label="Açıklama" fullWidth size="small" multiline rows={2} value={uF.Aciklama} onChange={e => setUF(p => ({ ...p, Aciklama: e.target.value }))} sx={{ mt: 2 }} />
          
          {modal === 'urun-ekle' && (
            <Box sx={{ mt: 3, p: 2, bgcolor: 'grey.50', borderRadius: 2 }}>
              <Typography variant="subtitle2" fontWeight={600} sx={{ mb: 1.5 }}>İlk Stok Girişi (Opsiyonel)</Typography>
              <Grid container spacing={2}>
                <Grid item xs={6}>
                  <FormControl fullWidth size="small"><InputLabel>Depo Seç</InputLabel>
                    <Select label="Depo Seç" value={uF.IlkDepoId} onChange={e => setUF(p => ({ ...p, IlkDepoId: e.target.value }))}>
                      <MenuItem value="">Şimdi belirleme</MenuItem>
                      {hDepolar.map(d => <MenuItem key={d.id} value={String(d.id)}>{d.depoAdi} ({d.bolge})</MenuItem>)}
                    </Select>
                  </FormControl>
                </Grid>
                <Grid item xs={6}>
                  <TextField label="Miktar" type="number" fullWidth size="small" disabled={!uF.IlkDepoId} value={uF.IlkMiktar}
                    onChange={e => setUF(p => ({ ...p, IlkMiktar: e.target.value }))} inputProps={{ min: 1 }} />
                </Grid>
              </Grid>
              <Typography variant="caption" color="text.secondary" sx={{ mt: 1, display: 'block' }}>
                İsterseniz ürünü oluşturduktan sonra da "Giriş/Çıkış Yap" butonundan stok girebilirsiniz.
              </Typography>
            </Box>
          )}
        </Box></DialogContent>
        <DialogActions sx={{ px: 3, py: 2 }}>
          <Button onClick={modalKapat} disabled={kaydYukl}>İptal</Button>
          <Button variant="contained" onClick={urunKaydet} disabled={kaydYukl} startIcon={kaydYukl ? <CircularProgress size={16} /> : undefined}>
            {kaydYukl ? 'Kaydediliyor…' : 'Kaydet'}
          </Button>
        </DialogActions>
      </Dialog>

      {/* ─── DEPO MODALI ─── */}
      <Dialog open={modal === 'depo-ekle' || modal === 'depo-duzenle'} onClose={modalKapat} maxWidth="sm" fullWidth>
        <DialogTitle sx={{ fontWeight: 700 }}>{modal === 'depo-ekle' ? 'Yeni Depo Ekle' : 'Depo Düzenle'}</DialogTitle>
        <DialogContent dividers><Box sx={{ pt: 1 }}>
          {modal === 'depo-ekle' && <TextField label="Depo Kodu *" required fullWidth size="small" value={dF.DepoKodu} onChange={e => setDF(p => ({ ...p, DepoKodu: e.target.value }))} sx={{ mb: 2 }} />}
          <TextField label="Depo Adı *" required fullWidth size="small" value={dF.DepoAdi} onChange={e => setDF(p => ({ ...p, DepoAdi: e.target.value }))} sx={{ mb: 2 }} />
          <Grid container spacing={2}>
            <Grid item xs={6}><TextField label="Bölge *" required fullWidth size="small" value={dF.Bolge} onChange={e => setDF(p => ({ ...p, Bolge: e.target.value }))} /></Grid>
            <Grid item xs={6}><TextField label="Raf *" required fullWidth size="small" value={dF.Raf} onChange={e => setDF(p => ({ ...p, Raf: e.target.value }))} /></Grid>
          </Grid>
          <Grid container spacing={2} sx={{ mt: 0 }}>
            <Grid item xs={6}><TextField label="Kapasite" type="number" fullWidth size="small" value={dF.Kapasite} onChange={e => setDF(p => ({ ...p, Kapasite: e.target.value }))} /></Grid>
            <Grid item xs={6}>
              <FormControl fullWidth size="small"><InputLabel>Durum</InputLabel>
                <Select label="Durum" value={dF.AktifMi ? 'aktif' : 'pasif'} onChange={e => setDF(p => ({ ...p, AktifMi: e.target.value === 'aktif' }))}>
                  <MenuItem value="aktif">Aktif</MenuItem>
                  <MenuItem value="pasif">Pasif</MenuItem>
                </Select>
              </FormControl>
            </Grid>
          </Grid>
          <TextField label="Konum / Adres" fullWidth size="small" value={dF.Konum} onChange={e => setDF(p => ({ ...p, Konum: e.target.value }))} sx={{ mt: 2 }} />
        </Box></DialogContent>
        <DialogActions sx={{ px: 3, py: 2 }}>
          <Button onClick={modalKapat} disabled={kaydYukl}>İptal</Button>
          <Button variant="contained" onClick={depoKaydet} disabled={kaydYukl} startIcon={kaydYukl ? <CircularProgress size={16} /> : undefined}>
            {kaydYukl ? 'Kaydediliyor…' : 'Kaydet'}
          </Button>
        </DialogActions>
      </Dialog>

      {/* ─── HAREKET MODALI ─── */}
      <Dialog open={modal === 'hareket'} onClose={modalKapat} maxWidth="sm" fullWidth>
        <DialogTitle sx={{ fontWeight: 700 }}>
          <SwapVertIcon sx={{ mr: 1, verticalAlign: 'middle', color: 'secondary.main' }} />
          Depo Giriş / Çıkış İşlemi
        </DialogTitle>
        <DialogContent dividers><Box sx={{ pt: 1 }}>
          <FormControl fullWidth size="small" sx={{ mb: 2 }}><InputLabel>İşlem Tipi *</InputLabel>
            <Select label="İşlem Tipi *" value={hF.HareketTipi} onChange={e => setHF(p => ({ ...p, HareketTipi: e.target.value }))}>
              <MenuItem value="Giris">▲ Depoya Giriş</MenuItem>
              <MenuItem value="Cikis">▼ Depodan Çıkış</MenuItem>
            </Select>
          </FormControl>
          <FormControl fullWidth size="small" sx={{ mb: 2 }}><InputLabel>Ürün *</InputLabel>
            <Select label="Ürün *" value={hF.UrunId} onChange={e => setHF(p => ({ ...p, UrunId: e.target.value }))}>
              {hUrunler.map(u => <MenuItem key={u.id} value={String(u.id)}>{u.urunKodu} — {u.urunAdi} (Stok: {u.toplamStok})</MenuItem>)}
            </Select>
          </FormControl>
          <FormControl fullWidth size="small" sx={{ mb: 2 }}><InputLabel>Depo *</InputLabel>
            <Select label="Depo *" value={hF.DepoId} onChange={e => setHF(p => ({ ...p, DepoId: e.target.value }))}>
              {hDepolar.map(d => <MenuItem key={d.id} value={String(d.id)}>{d.depoKodu} — {d.depoAdi} ({d.bolge}/{d.raf})</MenuItem>)}
            </Select>
          </FormControl>
          <TextField label="Miktar *" type="number" required fullWidth size="small" value={hF.Miktar} onChange={e => setHF(p => ({ ...p, Miktar: e.target.value }))} inputProps={{ min: 1 }} sx={{ mb: 2 }} />
          <Grid container spacing={2}>
            <Grid item xs={6}><TextField label="Referans No" fullWidth size="small" value={hF.ReferansNo} onChange={e => setHF(p => ({ ...p, ReferansNo: e.target.value }))} /></Grid>
            <Grid item xs={6}><TextField label="İşlemi Yapan" fullWidth size="small" value={hF.IslemYapan} onChange={e => setHF(p => ({ ...p, IslemYapan: e.target.value }))} /></Grid>
          </Grid>
          <TextField label="Açıklama" fullWidth size="small" multiline rows={2} value={hF.Aciklama} onChange={e => setHF(p => ({ ...p, Aciklama: e.target.value }))} sx={{ mt: 2 }} />
        </Box></DialogContent>
        <DialogActions sx={{ px: 3, py: 2 }}>
          <Button onClick={modalKapat} disabled={kaydYukl}>İptal</Button>
          <Button variant="contained" color="secondary" onClick={hareketKaydet} disabled={kaydYukl}
            startIcon={kaydYukl ? <CircularProgress size={16} /> : <SwapVertIcon />}>
            {kaydYukl ? 'İşleniyor…' : (hF.HareketTipi === 'Giris' ? 'Depoya Gir' : 'Depodan Çıkar')}
          </Button>
        </DialogActions>
      </Dialog>

      {/* ─── SİL ONAY MODALI ─── */}
      <Dialog open={silAcik} onClose={() => setSilAcik(false)} maxWidth="xs" fullWidth>
        <DialogTitle sx={{ fontWeight: 700, color: 'error.main' }}>Silme Onayı</DialogTitle>
        <DialogContent>
          <Typography>Bu {silTur === 'urun' ? 'ürünü' : 'depoyu'} silmek istediğinizden emin misiniz?</Typography>
        </DialogContent>
        <DialogActions sx={{ px: 3, py: 2 }}>
          <Button onClick={() => setSilAcik(false)} disabled={kaydYukl}>İptal</Button>
          <Button variant="contained" color="error" onClick={silOnayla} disabled={kaydYukl}
            startIcon={kaydYukl ? <CircularProgress size={16} /> : <DeleteIcon />}>
            {kaydYukl ? 'Siliniyor…' : 'Evet, Sil'}
          </Button>
        </DialogActions>
      </Dialog>

      {/* ─── BİLDİRİM ─── */}
      <Snackbar open={bildirim !== null} autoHideDuration={4000} onClose={() => setBildirim(null)}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}>
        {bildirim ? <Alert onClose={() => setBildirim(null)} severity={bildirim.t} variant="filled" sx={{ minWidth: 280 }}>{bildirim.m}</Alert> : <span />}
      </Snackbar>

    </Box>
  );
}
