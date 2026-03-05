import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { apiGet, apiPost, apiDelete, formatRupiah, formatDate } from '../../api/api';

export default function PengajuanDetails() {
    const { id } = useParams();
    const [pengajuan, setPengajuan] = useState(null);
    const [details, setDetails] = useState([]);
    const [loading, setLoading] = useState(true);
    const [msg, setMsg] = useState('');

    const loadData = () => {
        setLoading(true);
        apiGet(`/PengajuanApi/${id}`).then(data => {
            if (data) { setPengajuan(data.pengajuan); setDetails(data.details); }
        }).finally(() => setLoading(false));
    };

    useEffect(() => { loadData(); }, [id]);

    const handleSubmit = async () => {
        if (!confirm('Yakin ingin mengajukan pengajuan ini? Pengajuan yang sudah diajukan tidak dapat diubah.')) return;
        const res = await apiPost(`/PengajuanApi/${id}/submit`);
        if (res) { setMsg(res.message); loadData(); }
    };

    const handleDeleteDetail = async (detailId) => {
        if (!confirm('Apakah Anda yakin ingin menghapus data ini?')) return;
        const res = await apiDelete(`/DetailPengajuanApi/${detailId}`);
        if (res) loadData();
    };

    const handleMove = async (detailId, direction) => {
        await apiPost(`/DetailPengajuanApi/${detailId}/${direction}`);
        loadData();
    };

    if (loading) return <div className="text-center py-5"><i className="fas fa-spinner fa-spin fa-2x"></i></div>;
    if (!pengajuan) return <div className="text-center py-5">Data tidak ditemukan</div>;

    return (
        <div className="fade-in">
            {msg && (
                <div className="alert alert-success alert-dismissible fade show" role="alert">
                    <i className="fas fa-check-circle me-2"></i>{msg}
                    <button type="button" className="btn-close" onClick={() => setMsg('')}></button>
                </div>
            )}

            {/* Header Section */}
            <div className="detail-header">
                <div className="d-flex justify-content-between align-items-start flex-wrap gap-2">
                    <div>
                        <h2><i className="fas fa-file-invoice me-2"></i>Pengajuan Barang Modal</h2>
                        <span style={{ fontSize: '0.85rem', opacity: 0.9 }}>
                            ID: #{pengajuan.idPengajuan} |{' '}
                            {pengajuan.status === 'draft' && <span className="badge bg-warning text-dark">Draft</span>}
                            {pengajuan.status === 'approved' && <span className="badge bg-success">Diajukan</span>}
                        </span>
                    </div>
                    <Link to="/pengajuan" className="btn btn-sm btn-outline-light">
                        <i className="fas fa-arrow-left me-1"></i> Kembali
                    </Link>
                </div>

                <div className="detail-info-grid">
                    <div className="detail-info-item"><label>Tanggal Pengajuan</label><span>{formatDate(pengajuan.tanggalPengajuan, 'long')}</span></div>
                    <div className="detail-info-item"><label>Unit Kerja</label><span>{pengajuan.unitName}</span></div>
                    <div className="detail-info-item"><label>Total Harga</label><span>{formatRupiah(pengajuan.totalHarga)}</span></div>
                    <div className="detail-info-item"><label>Jumlah Item</label><span>{details.length} barang</span></div>
                </div>
            </div>

            {/* Surat Info */}
            <div className="info-card">
                <div className="info-card-title"><i className="fas fa-envelope"></i> Informasi Surat</div>
                <div className="info-grid">
                    <div className="info-item"><label>No. Surat Rektor</label><span>{pengajuan.noSuratRektor || '-'}</span></div>
                    <div className="info-item"><label>Tgl. Surat Rektor</label><span>{pengajuan.tglSuratRektor ? formatDate(pengajuan.tglSuratRektor) : 'Belum terkonfirmasi'}</span></div>
                    <div className="info-item"><label>Tahun Anggaran</label><span>{pengajuan.tahunAnggaran || '-'}</span></div>
                    <div className="info-item"><label>Jabatan Penandatangan</label><span>{pengajuan.jabatan || '-'}</span></div>
                    <div className="info-item"><label>Pejabat Penandatangan</label><span>{pengajuan.pejabatName || '-'}</span></div>
                    <div className="info-item"><label>Jenis Pengajuan</label><span>{pengajuan.jenisPengajuan || 'Belanja Modal'}</span></div>
                </div>
                {pengajuan.status === 'draft' && (
                    <div className="mt-3">
                        <Link to={`/pengajuan/edit/${pengajuan.idPengajuan}`} className="btn-sm-action btn-edit">
                            <i className="fas fa-pencil"></i> Edit Data Surat
                        </Link>
                    </div>
                )}
            </div>

            {/* Daftar Barang */}
            <div className="table-container">
                <div className="table-header">
                    <div className="table-title"><i className="fas fa-boxes-stacked me-2"></i>Daftar Barang Modal</div>
                    {pengajuan.status === 'draft' && (
                        <Link to={`/detailpengajuan/create/${pengajuan.idPengajuan}`} className="btn-primary-custom" style={{ fontSize: '0.8rem' }}>
                            <i className="fas fa-plus"></i> Tambah Barang
                        </Link>
                    )}
                </div>

                {details.length > 0 ? (
                    <div className="table-responsive">
                        <table className="table">
                            <thead>
                                <tr>
                                    <th style={{ width: 50 }}>No</th><th>Nama Barang</th><th>Spesifikasi</th>
                                    <th>Volume</th><th>Satuan</th><th>Harga Satuan</th><th>Jumlah Harga</th>
                                    <th>Lokasi</th><th>Asal</th>
                                    {pengajuan.status === 'draft' && <th style={{ width: 130 }}>Aksi</th>}
                                </tr>
                            </thead>
                            <tbody>
                                {details.map(item => (
                                    <tr key={item.idDetPengajuan}>
                                        <td><span className="badge bg-secondary">{item.noPrioritas}</span></td>
                                        <td><strong>{item.barangNama}</strong><br /><small className="text-muted">{item.barangKode}</small></td>
                                        <td style={{ maxWidth: 200, fontSize: '0.82rem', color: 'var(--text-secondary)' }}>{item.spesifikasi || '-'}</td>
                                        <td>{item.jumlahDiminta}</td>
                                        <td>Unit</td>
                                        <td className="currency">{formatRupiah(item.hargaSatuan)}</td>
                                        <td className="currency" style={{ color: 'var(--primary)', fontWeight: 700 }}>{formatRupiah(item.jumlahHarga)}</td>
                                        <td style={{ fontSize: '0.82rem' }}>{item.gedungNama}<br /><small className="text-muted">{item.ruangNama}</small></td>
                                        <td>
                                            {item.asalBarang === 'Import' ? <span className="badge bg-info">Import</span> : <span className="badge bg-success">PDN</span>}
                                        </td>
                                        {pengajuan.status === 'draft' && (
                                            <td>
                                                <div className="d-flex gap-1 flex-wrap">
                                                    <button className="btn-sm-action btn-move" title="Naikkan Prioritas" onClick={() => handleMove(item.idDetPengajuan, 'moveup')}><i className="fas fa-arrow-up"></i></button>
                                                    <button className="btn-sm-action btn-move" title="Turunkan Prioritas" onClick={() => handleMove(item.idDetPengajuan, 'movedown')}><i className="fas fa-arrow-down"></i></button>
                                                    <Link to={`/detailpengajuan/edit/${item.idDetPengajuan}`} className="btn-sm-action btn-edit" title="Edit"><i className="fas fa-pencil"></i></Link>
                                                    <button className="btn-sm-action btn-delete" title="Hapus" onClick={() => handleDeleteDetail(item.idDetPengajuan)}><i className="fas fa-trash"></i></button>
                                                </div>
                                            </td>
                                        )}
                                    </tr>
                                ))}
                            </tbody>
                            <tfoot>
                                <tr className="total-row">
                                    <td colSpan="6" style={{ textAlign: 'right', fontSize: '1rem' }}><strong>TOTAL</strong></td>
                                    <td className="currency" style={{ fontSize: '1.05rem' }}>{formatRupiah(pengajuan.totalHarga)}</td>
                                    <td colSpan={pengajuan.status === 'draft' ? 3 : 2}></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                ) : (
                    <div className="empty-state">
                        <i className="fas fa-box-open"></i>
                        <h3>Belum Ada Barang</h3>
                        <p>Belum ada barang yang ditambahkan ke pengajuan ini.</p>
                        <Link to={`/detailpengajuan/create/${pengajuan.idPengajuan}`} className="btn-primary-custom">
                            <i className="fas fa-plus-circle"></i> Tambah Barang Pertama
                        </Link>
                    </div>
                )}
            </div>

            {/* Action Buttons */}
            {pengajuan.status === 'draft' && details.length > 0 && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem' }} onClick={handleSubmit}>
                        <i className="fas fa-paper-plane"></i> Ajukan Pengajuan
                    </button>
                </div>
            )}
        </div>
    );
}
