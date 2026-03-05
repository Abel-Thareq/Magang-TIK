import { useState, useEffect } from 'react';
import { apiGet, apiPost, apiPut, apiDelete } from '../../api/api';

export default function KodeBarangIndex() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [activeTab, setActiveTab] = useState('Golongan');
    const [msg, setMsg] = useState({ text: '', type: '' });

    // Filters
    const [filterGolongan, setFilterGolongan] = useState('');
    const [filterBidang, setFilterBidang] = useState('');
    const [filterKelompok, setFilterKelompok] = useState('');
    const [filterSubKelompok, setFilterSubKelompok] = useState('');
    const [filterLevel, setFilterLevel] = useState('');
    const [searchText, setSearchText] = useState('');

    // Cascading dropdown data
    const [golongans, setGolongans] = useState([]);
    const [bidangs, setBidangs] = useState([]);
    const [kelompoks, setKelompoks] = useState([]);
    const [subKelompoks, setSubKelompoks] = useState([]);

    // Create form
    const [createForm, setCreateForm] = useState({
        kodeGolongan: '', kodeBidang: '', kodeKelompok: '', kodeSubKelompok: '', kodeBarangValue: '', uraianBarang: ''
    });

    // Edit modal
    const [editItem, setEditItem] = useState(null);
    const [editUraian, setEditUraian] = useState('');

    const loadData = () => {
        setLoading(true);
        let params = new URLSearchParams();
        if (filterGolongan) params.set('filterGolongan', filterGolongan);
        if (filterBidang) params.set('filterBidang', filterBidang);
        if (filterKelompok) params.set('filterKelompok', filterKelompok);
        if (filterSubKelompok) params.set('filterSubKelompok', filterSubKelompok);
        if (filterLevel) params.set('filterLevel', filterLevel);

        apiGet(`/KodeBarangApi?${params.toString()}`).then(res => {
            if (res) setData(res);
        }).finally(() => setLoading(false));
    };

    useEffect(() => { loadData(); }, [filterGolongan, filterBidang, filterKelompok, filterSubKelompok, filterLevel]);

    useEffect(() => {
        apiGet('/KodeBarangApi/Golongan').then(res => res && setGolongans(res));
    }, []);

    // Cascading for filter
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

    // Cascading for create form
    const [cGolongans, setCGolongans] = useState([]);
    const [cBidangs, setCBidangs] = useState([]);
    const [cKelompoks, setCKelompoks] = useState([]);
    const [cSubKelompoks, setCSubKelompoks] = useState([]);

    useEffect(() => {
        apiGet('/KodeBarangApi/Golongan').then(r => r && setCGolongans(r));
    }, []);

    const handleCreate = async (e) => {
        e.preventDefault();
        const body = { level: activeTab, ...createForm };
        const res = await apiPost('/KodeBarangApi', body);
        if (res?.message) {
            setMsg({ text: res.message, type: res.id ? 'success' : 'danger' });
            if (res.id) { setCreateForm({ kodeGolongan: '', kodeBidang: '', kodeKelompok: '', kodeSubKelompok: '', kodeBarangValue: '', uraianBarang: '' }); loadData(); }
        }
    };

    const handleEdit = async () => {
        if (!editItem) return;
        const res = await apiPut(`/KodeBarangApi/${editItem.id}`, { uraianBarang: editUraian });
        if (res?.message) { setMsg({ text: res.message, type: 'success' }); setEditItem(null); loadData(); }
    };

    const handleDelete = async (id) => {
        if (!confirm('Yakin ingin menghapus kode barang ini?')) return;
        const res = await apiDelete(`/KodeBarangApi/${id}`);
        if (res?.message) { setMsg({ text: res.message, type: 'success' }); loadData(); }
    };

    const filteredData = searchText ? data.filter(d => d.uraianBarang.toLowerCase().includes(searchText.toLowerCase()) || d.kodeBarangLengkap.includes(searchText)) : data;

    const tabs = ['Golongan', 'Bidang', 'Kelompok', 'SubKelompok', 'KodeBarang'];

    const getLevelBadge = (level) => {
        const colors = { Golongan: '#ca8a04', Bidang: '#1565C0', Kelompok: '#6A1B9A', 'Sub Kelompok': '#E65100', 'Kode Barang': '#C62828' };
        return <span className="badge" style={{ background: colors[level] || '#666', fontSize: '0.7rem' }}>{level}</span>;
    };

    return (
        <div className="fade-in">
            {msg.text && (
                <div className={`alert alert-${msg.type} alert-dismissible fade show`} role="alert">
                    {msg.text}
                    <button type="button" className="btn-close" onClick={() => setMsg({ text: '', type: '' })}></button>
                </div>
            )}

            {/* Tambah Kode Barang Tabs */}
            <div className="card mb-4">
                <div className="card-header d-flex align-items-center gap-2">
                    <i className="fas fa-plus-circle" style={{ color: 'var(--primary)' }}></i>
                    <strong>Tambah Kode Barang</strong>
                </div>
                <div className="card-body">
                    <ul className="nav nav-tabs mb-3">
                        {tabs.map(tab => (
                            <li className="nav-item" key={tab}>
                                <button className={`nav-link ${activeTab === tab ? 'active' : ''}`} onClick={() => setActiveTab(tab)} style={activeTab === tab ? { color: '#1a1a2e', fontWeight: 600 } : { color: '#1a1a2e' }}>
                                    {tab === 'SubKelompok' ? 'Sub Kelompok' : tab === 'KodeBarang' ? 'Kode Barang' : tab}
                                </button>
                            </li>
                        ))}
                    </ul>

                    <form onSubmit={handleCreate}>
                        <div className="row g-3">
                            <div className="col-md-2">
                                <label className="form-label">Kode Golongan <span className="required-star">*</span></label>
                                {activeTab === 'Golongan' ? (
                                    <input className="form-control" required maxLength="1" pattern="\d{1}" placeholder="0" value={createForm.kodeGolongan} onChange={e => setCreateForm({ ...createForm, kodeGolongan: e.target.value })} />
                                ) : (
                                    <select className="form-select" required value={createForm.kodeGolongan} onChange={e => {
                                        setCreateForm({ ...createForm, kodeGolongan: e.target.value, kodeBidang: '', kodeKelompok: '', kodeSubKelompok: '' });
                                        setCBidangs([]); setCKelompoks([]); setCSubKelompoks([]);
                                        if (e.target.value) apiGet(`/KodeBarangApi/Bidang?kodeGolongan=${e.target.value}`).then(r => r && setCBidangs(r));
                                    }}>
                                        <option value="">Pilih</option>
                                        {cGolongans.map(g => <option key={g.kode} value={g.kode}>{g.kode} - {g.uraian}</option>)}
                                    </select>
                                )}
                            </div>

                            {activeTab !== 'Golongan' && (
                                <div className="col-md-2">
                                    <label className="form-label">Kode Bidang {activeTab === 'Bidang' && <span className="required-star">*</span>}</label>
                                    {activeTab === 'Bidang' ? (
                                        <input className="form-control" required maxLength="2" pattern="\d{2}" placeholder="00" value={createForm.kodeBidang} onChange={e => setCreateForm({ ...createForm, kodeBidang: e.target.value })} />
                                    ) : (
                                        <select className="form-select" required value={createForm.kodeBidang} onChange={e => {
                                            setCreateForm({ ...createForm, kodeBidang: e.target.value, kodeKelompok: '', kodeSubKelompok: '' });
                                            setCKelompoks([]); setCSubKelompoks([]);
                                            if (e.target.value) apiGet(`/KodeBarangApi/Kelompok?kodeGolongan=${createForm.kodeGolongan}&kodeBidang=${e.target.value}`).then(r => r && setCKelompoks(r));
                                        }}>
                                            <option value="">Pilih</option>
                                            {cBidangs.map(b => <option key={b.kode} value={b.kode}>{b.kode} - {b.uraian}</option>)}
                                        </select>
                                    )}
                                </div>
                            )}

                            {(activeTab === 'Kelompok' || activeTab === 'SubKelompok' || activeTab === 'KodeBarang') && (
                                <div className="col-md-2">
                                    <label className="form-label">Kode Kelompok {activeTab === 'Kelompok' && <span className="required-star">*</span>}</label>
                                    {activeTab === 'Kelompok' ? (
                                        <input className="form-control" required maxLength="2" pattern="\d{2}" placeholder="00" value={createForm.kodeKelompok} onChange={e => setCreateForm({ ...createForm, kodeKelompok: e.target.value })} />
                                    ) : (
                                        <select className="form-select" required value={createForm.kodeKelompok} onChange={e => {
                                            setCreateForm({ ...createForm, kodeKelompok: e.target.value, kodeSubKelompok: '' });
                                            setCSubKelompoks([]);
                                            if (e.target.value) apiGet(`/KodeBarangApi/SubKelompok?kodeGolongan=${createForm.kodeGolongan}&kodeBidang=${createForm.kodeBidang}&kodeKelompok=${e.target.value}`).then(r => r && setCSubKelompoks(r));
                                        }}>
                                            <option value="">Pilih</option>
                                            {cKelompoks.map(k => <option key={k.kode} value={k.kode}>{k.kode} - {k.uraian}</option>)}
                                        </select>
                                    )}
                                </div>
                            )}

                            {(activeTab === 'SubKelompok' || activeTab === 'KodeBarang') && (
                                <div className="col-md-2">
                                    <label className="form-label">Kode Sub Kelompok {activeTab === 'SubKelompok' && <span className="required-star">*</span>}</label>
                                    {activeTab === 'SubKelompok' ? (
                                        <input className="form-control" required maxLength="2" pattern="\d{2}" placeholder="00" value={createForm.kodeSubKelompok} onChange={e => setCreateForm({ ...createForm, kodeSubKelompok: e.target.value })} />
                                    ) : (
                                        <select className="form-select" required value={createForm.kodeSubKelompok} onChange={e => setCreateForm({ ...createForm, kodeSubKelompok: e.target.value })}>
                                            <option value="">Pilih</option>
                                            {cSubKelompoks.map(s => <option key={s.kode} value={s.kode}>{s.kode} - {s.uraian}</option>)}
                                        </select>
                                    )}
                                </div>
                            )}

                            {activeTab === 'KodeBarang' && (
                                <div className="col-md-2">
                                    <label className="form-label">Kode Barang <span className="required-star">*</span></label>
                                    <input className="form-control" required maxLength="3" pattern="\d{3}" placeholder="000" value={createForm.kodeBarangValue} onChange={e => setCreateForm({ ...createForm, kodeBarangValue: e.target.value })} />
                                </div>
                            )}

                            <div className="col-md-3">
                                <label className="form-label">Uraian Barang <span className="required-star">*</span></label>
                                <input className="form-control" required value={createForm.uraianBarang} onChange={e => setCreateForm({ ...createForm, uraianBarang: e.target.value })} />
                            </div>

                            <div className="col-md-2 d-flex align-items-end">
                                <button type="submit" className="btn-primary-custom w-100" style={{ padding: '0.6rem', fontSize: '0.8rem' }}>
                                    <i className="fas fa-plus"></i> Tambah
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>

            {/* Filter Section */}
            <div className="card mb-4">
                <div className="card-header d-flex align-items-center gap-2">
                    <i className="fas fa-filter" style={{ color: 'var(--primary)' }}></i>
                    <strong>Filter</strong>
                </div>
                <div className="card-body">
                    <div className="row g-3">
                        <div className="col-md-2">
                            <label className="form-label" style={{ fontSize: '0.8rem' }}>Golongan</label>
                            <select className="form-select form-select-sm" value={filterGolongan} onChange={e => handleFilterGolonganChange(e.target.value)}>
                                <option value="">Semua</option>
                                {golongans.map(g => <option key={g.kode} value={g.kode}>{g.kode} - {g.uraian}</option>)}
                            </select>
                        </div>
                        <div className="col-md-2">
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
                            <label className="form-label" style={{ fontSize: '0.8rem' }}>Level</label>
                            <select className="form-select form-select-sm" value={filterLevel} onChange={e => setFilterLevel(e.target.value)}>
                                <option value="">Semua</option>
                                <option value="Golongan">Golongan</option>
                                <option value="Bidang">Bidang</option>
                                <option value="Kelompok">Kelompok</option>
                                <option value="SubKelompok">Sub Kelompok</option>
                                <option value="KodeBarang">Kode Barang</option>
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
                    <div className="table-title">Data Kode Barang</div>
                    <span style={{ fontSize: '0.8rem', color: 'var(--text-secondary)' }}>{filteredData.length} data</span>
                </div>
                {loading ? (
                    <div className="text-center py-4"><i className="fas fa-spinner fa-spin fa-2x"></i></div>
                ) : (
                    <div className="table-responsive">
                        <table className="table">
                            <thead>
                                <tr><th>No</th><th>Kode Barang</th><th>Uraian</th><th>Level</th><th>Aksi</th></tr>
                            </thead>
                            <tbody>
                                {filteredData.map((item, i) => (
                                    <tr key={item.id}>
                                        <td>{i + 1}</td>
                                        <td><code style={{ fontSize: '0.85rem', fontWeight: 600 }}>{item.kodeBarangLengkap}</code></td>
                                        <td>{item.uraianBarang}</td>
                                        <td>{getLevelBadge(item.level)}</td>
                                        <td>
                                            <div className="d-flex gap-1">
                                                <button className="btn-sm-action btn-edit" onClick={() => { setEditItem(item); setEditUraian(item.uraianBarang); }}>
                                                    <i className="fas fa-pencil"></i>
                                                </button>
                                                <button className="btn-sm-action btn-delete" onClick={() => handleDelete(item.id)}>
                                                    <i className="fas fa-trash"></i>
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                                {filteredData.length === 0 && (
                                    <tr><td colSpan="5" className="text-center py-3 text-muted">Tidak ada data yang ditemukan.</td></tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>

            {/* Edit Modal */}
            {editItem && (
                <div className="modal show d-block" style={{ background: 'rgba(0,0,0,0.4)' }} onClick={() => setEditItem(null)}>
                    <div className="modal-dialog" onClick={e => e.stopPropagation()}>
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Edit Kode Barang</h5>
                                <button type="button" className="btn-close" onClick={() => setEditItem(null)}></button>
                            </div>
                            <div className="modal-body">
                                <div className="mb-3">
                                    <label className="form-label">Kode</label>
                                    <input className="form-control" disabled value={editItem.kodeBarangLengkap} />
                                </div>
                                <div className="mb-3">
                                    <label className="form-label">Uraian Barang</label>
                                    <input className="form-control" value={editUraian} onChange={e => setEditUraian(e.target.value)} />
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-secondary" onClick={() => setEditItem(null)}>Batal</button>
                                <button className="btn-primary-custom" onClick={handleEdit}>
                                    <i className="fas fa-save"></i> Simpan
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
