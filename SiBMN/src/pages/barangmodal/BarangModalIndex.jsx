import { useState, useEffect } from 'react';
import { apiGet } from '../../api/api';

export default function BarangModalIndex() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchText, setSearchText] = useState('');

    // Filters
    const [filterGolongan, setFilterGolongan] = useState('');
    const [filterBidang, setFilterBidang] = useState('');
    const [filterKelompok, setFilterKelompok] = useState('');
    const [filterSubKelompok, setFilterSubKelompok] = useState('');

    const [golongans, setGolongans] = useState([]);
    const [bidangs, setBidangs] = useState([]);
    const [kelompoks, setKelompoks] = useState([]);
    const [subKelompoks, setSubKelompoks] = useState([]);

    const loadData = () => {
        setLoading(true);
        let params = new URLSearchParams();
        if (filterGolongan) params.set('filterGolongan', filterGolongan);
        if (filterBidang) params.set('filterBidang', filterBidang);
        if (filterKelompok) params.set('filterKelompok', filterKelompok);
        if (filterSubKelompok) params.set('filterSubKelompok', filterSubKelompok);
        params.set('filterLevel', 'KodeBarang');

        apiGet(`/KodeBarangApi?${params.toString()}`).then(res => {
            if (res) setData(res);
        }).finally(() => setLoading(false));
    };

    useEffect(() => { loadData(); }, [filterGolongan, filterBidang, filterKelompok, filterSubKelompok]);

    useEffect(() => {
        apiGet('/KodeBarangApi/Golongan').then(res => res && setGolongans(res));
    }, []);

    const handleFilterGolonganChange = (val) => {
        setFilterGolongan(val);
        setFilterBidang(''); setFilterKelompok(''); setFilterSubKelompok('');
        setBidangs([]); setKelompoks([]); setSubKelompoks([]);
        if (val) apiGet(`/KodeBarangApi/Bidang?kodeGolongan=${val}`).then(r => r && setBidangs(r));
    };

    const handleFilterBidangChange = (val) => {
        setFilterBidang(val);
        setFilterKelompok(''); setFilterSubKelompok('');
        setKelompoks([]); setSubKelompoks([]);
        if (val) apiGet(`/KodeBarangApi/Kelompok?kodeGolongan=${filterGolongan}&kodeBidang=${val}`).then(r => r && setKelompoks(r));
    };

    const handleFilterKelompokChange = (val) => {
        setFilterKelompok(val);
        setFilterSubKelompok('');
        setSubKelompoks([]);
        if (val) apiGet(`/KodeBarangApi/SubKelompok?kodeGolongan=${filterGolongan}&kodeBidang=${filterBidang}&kodeKelompok=${val}`).then(r => r && setSubKelompoks(r));
    };

    const filteredData = searchText ? data.filter(d => d.uraianBarang.toLowerCase().includes(searchText.toLowerCase()) || d.kodeBarangLengkap.includes(searchText)) : data;

    return (
        <div className="fade-in">
            <div className="mb-4">
                <h2 style={{ fontSize: '1.3rem', fontWeight: 700 }}>Daftar Barang Modal</h2>
                <p style={{ color: 'var(--text-secondary)', fontSize: '0.875rem' }}>
                    Lihat seluruh data barang modal berdasarkan kode barang
                </p>
            </div>

            {/* Filter Section */}
            <div className="card mb-4">
                <div className="card-header d-flex align-items-center gap-2">
                    <i className="fas fa-filter" style={{ color: 'var(--primary)' }}></i>
                    <strong>Filter</strong>
                </div>
                <div className="card-body">
                    <div className="row g-3">
                        <div className="col-md-3">
                            <label className="form-label" style={{ fontSize: '0.8rem' }}>Golongan</label>
                            <select className="form-select form-select-sm" value={filterGolongan} onChange={e => handleFilterGolonganChange(e.target.value)}>
                                <option value="">Semua</option>
                                {golongans.map(g => <option key={g.kode} value={g.kode}>{g.kode} - {g.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-3">
                            <label className="form-label" style={{ fontSize: '0.8rem' }}>Bidang</label>
                            <select className="form-select form-select-sm" value={filterBidang} onChange={e => handleFilterBidangChange(e.target.value)} disabled={!filterGolongan}>
                                <option value="">Semua</option>
                                {bidangs.map(b => <option key={b.kode} value={b.kode}>{b.kode} - {b.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-2">
                            <label className="form-label" style={{ fontSize: '0.8rem' }}>Kelompok</label>
                            <select className="form-select form-select-sm" value={filterKelompok} onChange={e => handleFilterKelompokChange(e.target.value)} disabled={!filterBidang}>
                                <option value="">Semua</option>
                                {kelompoks.map(k => <option key={k.kode} value={k.kode}>{k.kode} - {k.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-2">
                            <label className="form-label" style={{ fontSize: '0.8rem' }}>Sub Kelompok</label>
                            <select className="form-select form-select-sm" value={filterSubKelompok} onChange={e => setFilterSubKelompok(e.target.value)} disabled={!filterKelompok}>
                                <option value="">Semua</option>
                                {subKelompoks.map(s => <option key={s.kode} value={s.kode}>{s.kode} - {s.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-2">
                            <label className="form-label" style={{ fontSize: '0.8rem' }}>Cari</label>
                            <input className="form-control form-control-sm" placeholder="Cari uraian..." value={searchText} onChange={e => setSearchText(e.target.value)} />
                        </div>
                    </div>
                </div>
            </div>

            {/* Table */}
            <div className="table-container">
                <div className="table-header">
                    <div className="table-title">Data Barang Modal</div>
                    <span style={{ fontSize: '0.8rem', color: 'var(--text-secondary)' }}>{filteredData.length} data</span>
                </div>
                {loading ? (
                    <div className="text-center py-4"><i className="fas fa-spinner fa-spin fa-2x"></i></div>
                ) : (
                    <div className="table-responsive">
                        <table className="table">
                            <thead>
                                <tr><th>No</th><th>Kode Barang</th><th>Uraian Barang</th></tr>
                            </thead>
                            <tbody>
                                {filteredData.map((item, i) => (
                                    <tr key={item.id}>
                                        <td>{i + 1}</td>
                                        <td><code style={{ fontSize: '0.85rem', fontWeight: 600 }}>{item.kodeBarangLengkap}</code></td>
                                        <td>{item.uraianBarang}</td>
                                    </tr>
                                ))}
                                {filteredData.length === 0 && (
                                    <tr><td colSpan="3" className="text-center py-3 text-muted">Tidak ada data barang modal.</td></tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        </div>
    );
}
