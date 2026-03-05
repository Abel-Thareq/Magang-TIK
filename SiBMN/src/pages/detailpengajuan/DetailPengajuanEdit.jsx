import { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { apiGet, apiPut } from '../../api/api';

export default function DetailPengajuanEdit() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [gedungs, setGedungs] = useState([]);
    const [ruangs, setRuangs] = useState([]);
    const [pengajuanId, setPengajuanId] = useState(null);
    const [loading, setLoading] = useState(false);
    const [dataLoading, setDataLoading] = useState(true);

    // Barang selection with cascading filter
    const [barangs, setBarangs] = useState([]);
    const [barangSearch, setBarangSearch] = useState('');
    const [barangLoading, setBarangLoading] = useState(false);
    const [fGolongan, setFGolongan] = useState('');
    const [fBidang, setFBidang] = useState('');
    const [fKelompok, setFKelompok] = useState('');
    const [fSubKelompok, setFSubKelompok] = useState('');
    const [golongans, setGolongans] = useState([]);
    const [bidangs, setBidangs] = useState([]);
    const [kelompoks, setKelompoks] = useState([]);
    const [subKelompoks, setSubKelompoks] = useState([]);

    const [form, setForm] = useState({
        idBarang: '', spesifikasi: '', jumlahDiminta: 1, hargaSatuan: '',
        idRuang: '', fungsiBarang: '', asalBarang: 'PDN', alasanImport: '',
        linkSurvey: '', linkGambar: '', gedung: '',
    });

    const loadBarangs = (golongan, bidang, kelompok, subKelompok, search) => {
        setBarangLoading(true);
        let params = new URLSearchParams();
        if (golongan) params.set('golongan', golongan);
        if (bidang) params.set('bidang', bidang);
        if (kelompok) params.set('kelompok', kelompok);
        if (subKelompok) params.set('subKelompok', subKelompok);
        if (search) params.set('search', search);
        apiGet(`/DetailPengajuanApi/kodebarangs?${params.toString()}`).then(data => {
            if (data) setBarangs(data);
        }).finally(() => setBarangLoading(false));
    };

    useEffect(() => {
        apiGet('/KodeBarangApi/Golongan').then(r => r && setGolongans(r));
        loadBarangs('', '', '', '', '');

        Promise.all([
            apiGet('/DetailPengajuanApi/dropdowns'),
            apiGet(`/DetailPengajuanApi/${id}`)
        ]).then(([dropdowns, detail]) => {
            if (dropdowns) setGedungs(dropdowns.gedungs || []);
            if (detail) {
                setPengajuanId(detail.idPengajuan);
                setForm({
                    idBarang: detail.idBarang || '',
                    spesifikasi: detail.spesifikasi || '',
                    jumlahDiminta: detail.jumlahDiminta || 1,
                    hargaSatuan: detail.hargaSatuan || '',
                    idRuang: detail.idRuang || '',
                    fungsiBarang: detail.fungsiBarang || '',
                    asalBarang: detail.asalBarang || 'PDN',
                    alasanImport: detail.alasanImport || '',
                    linkSurvey: detail.linkSurvey || '',
                    linkGambar: detail.linkGambar || '',
                    gedung: detail.gedungNama || '',
                });
                if (detail.gedungNama) {
                    apiGet(`/BarangApi/GetRuangs?gedung=${encodeURIComponent(detail.gedungNama)}`).then(r => r && setRuangs(r));
                }
            }
        }).finally(() => setDataLoading(false));
    }, [id]);

    const handleFGolonganChange = (val) => {
        setFGolongan(val); setFBidang(''); setFKelompok(''); setFSubKelompok('');
        setBidangs([]); setKelompoks([]); setSubKelompoks([]);
        if (val) apiGet(`/KodeBarangApi/Bidang?kodeGolongan=${val}`).then(r => r && setBidangs(r));
        loadBarangs(val, '', '', '', barangSearch);
    };

    const handleFBidangChange = (val) => {
        setFBidang(val); setFKelompok(''); setFSubKelompok('');
        setKelompoks([]); setSubKelompoks([]);
        if (val) apiGet(`/KodeBarangApi/Kelompok?kodeGolongan=${fGolongan}&kodeBidang=${val}`).then(r => r && setKelompoks(r));
        loadBarangs(fGolongan, val, '', '', barangSearch);
    };

    const handleFKelompokChange = (val) => {
        setFKelompok(val); setFSubKelompok('');
        setSubKelompoks([]);
        if (val) apiGet(`/KodeBarangApi/SubKelompok?kodeGolongan=${fGolongan}&kodeBidang=${fBidang}&kodeKelompok=${val}`).then(r => r && setSubKelompoks(r));
        loadBarangs(fGolongan, fBidang, val, '', barangSearch);
    };

    const handleFSubKelompokChange = (val) => {
        setFSubKelompok(val);
        loadBarangs(fGolongan, fBidang, fKelompok, val, barangSearch);
    };

    const handleBarangSearch = (val) => {
        setBarangSearch(val);
        loadBarangs(fGolongan, fBidang, fKelompok, fSubKelompok, val);
    };

    const handleGedungChange = async (gedung) => {
        setForm({ ...form, gedung, idRuang: '' });
        if (gedung) {
            const data = await apiGet(`/BarangApi/GetRuangs?gedung=${encodeURIComponent(gedung)}`);
            if (data) setRuangs(data);
        } else setRuangs([]);
    };

    const jumlahHarga = (Number(form.jumlahDiminta) || 0) * (Number(form.hargaSatuan) || 0);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        await apiPut(`/DetailPengajuanApi/${id}`, {
            idPengajuan: Number(pengajuanId),
            idBarang: Number(form.idBarang),
            spesifikasi: form.spesifikasi,
            jumlahDiminta: Number(form.jumlahDiminta),
            hargaSatuan: Number(form.hargaSatuan),
            idRuang: Number(form.idRuang),
            fungsiBarang: form.fungsiBarang,
            asalBarang: form.asalBarang,
            alasanImport: form.alasanImport,
            linkSurvey: form.linkSurvey,
            linkGambar: form.linkGambar,
        });
        setLoading(false);
        navigate(`/pengajuan/${pengajuanId}`);
    };

    if (dataLoading) return <div className="text-center py-5"><i className="fas fa-spinner fa-spin fa-2x"></i></div>;

    return (
        <div className="fade-in">
            <Link to={`/pengajuan/${pengajuanId}`} className="btn-outline-custom mb-3" style={{ fontSize: '0.8rem' }}>
                <i className="fas fa-arrow-left"></i> Kembali ke Detail Pengajuan
            </Link>

            <form onSubmit={handleSubmit}>
                {/* Barang Selection with Filter */}
                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-box"></i> Pilih Barang</div>

                    {/* Cascading Filter */}
                    <div className="row g-2 mb-3">
                        <div className="col-md-12">
                            <label className="form-label" style={{ fontSize: '0.8rem', color: 'var(--text-secondary)' }}>
                                <i className="fas fa-filter me-1"></i> Filter berdasarkan kategori (opsional):
                            </label>
                        </div>
                        <div className="col-md-2">
                            <select className="form-select form-select-sm" value={fGolongan} onChange={e => handleFGolonganChange(e.target.value)}>
                                <option value="">Semua Golongan</option>
                                {golongans.map(g => <option key={g.kode} value={g.kode}>{g.kode} - {g.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-2">
                            <select className="form-select form-select-sm" value={fBidang} onChange={e => handleFBidangChange(e.target.value)} disabled={!fGolongan}>
                                <option value="">Semua Bidang</option>
                                {bidangs.map(b => <option key={b.kode} value={b.kode}>{b.kode} - {b.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-2">
                            <select className="form-select form-select-sm" value={fKelompok} onChange={e => handleFKelompokChange(e.target.value)} disabled={!fBidang}>
                                <option value="">Semua Kelompok</option>
                                {kelompoks.map(k => <option key={k.kode} value={k.kode}>{k.kode} - {k.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-2">
                            <select className="form-select form-select-sm" value={fSubKelompok} onChange={e => handleFSubKelompokChange(e.target.value)} disabled={!fKelompok}>
                                <option value="">Semua Sub Kelompok</option>
                                {subKelompoks.map(s => <option key={s.kode} value={s.kode}>{s.kode} - {s.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-4">
                            <div className="input-group input-group-sm">
                                <span className="input-group-text"><i className="fas fa-search"></i></span>
                                <input className="form-control form-control-sm" placeholder="Cari nama barang..." value={barangSearch} onChange={e => handleBarangSearch(e.target.value)} />
                            </div>
                        </div>
                    </div>

                    {/* Barang Dropdown */}
                    <div className="row g-3">
                        <div className="col-md-12">
                            <label className="form-label">Pilih Barang <span className="required-star">*</span></label>
                            <select className="form-select" required value={form.idBarang} onChange={e => setForm({ ...form, idBarang: e.target.value })}>
                                <option value="">-- Pilih Barang ({barangLoading ? 'Memuat...' : `${barangs.length} tersedia`}) --</option>
                                {barangs.map(b => <option key={b.id} value={b.id}>{b.display}</option>)}
                            </select>
                            <div className="form-text">Gunakan filter di atas untuk mempersempit pilihan barang, atau cari berdasarkan nama</div>
                        </div>
                        <div className="col-md-12">
                            <label className="form-label">Spesifikasi</label>
                            <textarea className="form-control" rows="3" value={form.spesifikasi} onChange={e => setForm({ ...form, spesifikasi: e.target.value })} />
                        </div>
                    </div>
                </div>

                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-calculator"></i> Volume dan Harga</div>
                    <div className="row g-3">
                        <div className="col-md-4">
                            <label className="form-label">Volume <span className="required-star">*</span></label>
                            <input type="number" className="form-control" required min="1" value={form.jumlahDiminta} onChange={e => setForm({ ...form, jumlahDiminta: e.target.value })} />
                        </div>
                        <div className="col-md-4">
                            <label className="form-label">Harga Satuan (Rp) <span className="required-star">*</span></label>
                            <input type="number" className="form-control" required min="1" value={form.hargaSatuan} onChange={e => setForm({ ...form, hargaSatuan: e.target.value })} />
                        </div>
                        <div className="col-md-4">
                            <label className="form-label">Jumlah Harga</label>
                            <input className="form-control" disabled value={`Rp ${jumlahHarga.toLocaleString('id-ID')}`} />
                        </div>
                    </div>
                </div>

                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-map-marker-alt"></i> Lokasi Penempatan</div>
                    <div className="row g-3">
                        <div className="col-md-6">
                            <label className="form-label">Gedung</label>
                            <select className="form-select" value={form.gedung} onChange={e => handleGedungChange(e.target.value)}>
                                <option value="">-- Pilih Gedung --</option>
                                {gedungs.map(g => <option key={g} value={g}>{g}</option>)}
                            </select>
                        </div>
                        <div className="col-md-6">
                            <label className="form-label">Ruang <span className="required-star">*</span></label>
                            <select className="form-select" required value={form.idRuang} onChange={e => setForm({ ...form, idRuang: e.target.value })}>
                                <option value="">-- Pilih Ruang --</option>
                                {ruangs.map(r => <option key={r.idRuang} value={r.idRuang}>{r.namaRuang}</option>)}
                            </select>
                        </div>
                        <div className="col-md-12">
                            <label className="form-label">Fungsi Barang</label>
                            <textarea className="form-control" rows="2" value={form.fungsiBarang} onChange={e => setForm({ ...form, fungsiBarang: e.target.value })} />
                        </div>
                    </div>
                </div>

                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-globe"></i> Asal Barang</div>
                    <div className="row g-3">
                        <div className="col-md-12">
                            <div className="d-flex gap-4">
                                <label className="d-flex align-items-center gap-2">
                                    <input type="radio" name="asalBarang" value="PDN" checked={form.asalBarang === 'PDN'} onChange={e => setForm({ ...form, asalBarang: e.target.value })} />
                                    <span>PDN</span>
                                </label>
                                <label className="d-flex align-items-center gap-2">
                                    <input type="radio" name="asalBarang" value="Import" checked={form.asalBarang === 'Import'} onChange={e => setForm({ ...form, asalBarang: e.target.value })} />
                                    <span>Import</span>
                                </label>
                            </div>
                        </div>
                        {form.asalBarang === 'Import' && (
                            <div className="col-md-12">
                                <label className="form-label">Alasan Import</label>
                                <textarea className="form-control" rows="2" value={form.alasanImport} onChange={e => setForm({ ...form, alasanImport: e.target.value })} />
                            </div>
                        )}
                    </div>
                </div>

                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-link"></i> Link Referensi</div>
                    <div className="row g-3">
                        <div className="col-md-6">
                            <label className="form-label">Link Survey (e-Katalog)</label>
                            <input className="form-control" type="url" value={form.linkSurvey} onChange={e => setForm({ ...form, linkSurvey: e.target.value })} />
                        </div>
                        <div className="col-md-6">
                            <label className="form-label">Link Gambar</label>
                            <input className="form-control" type="url" value={form.linkGambar} onChange={e => setForm({ ...form, linkGambar: e.target.value })} />
                        </div>
                    </div>
                </div>

                <div className="d-flex gap-3">
                    <button type="submit" className="btn-primary-custom" disabled={loading}>
                        <i className="fas fa-save"></i> {loading ? 'Menyimpan...' : 'Simpan Perubahan'}
                    </button>
                    <Link to={`/pengajuan/${pengajuanId}`} className="btn-outline-custom">
                        <i className="fas fa-times"></i> Batal
                    </Link>
                </div>
            </form>
        </div>
    );
}
