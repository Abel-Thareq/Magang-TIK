import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { apiGet, apiPatch, apiDelete, formatRupiah, formatDate } from '../../api/api';

export default function PengajuanIndex() {
    const { user } = useAuth();
    const navigate = useNavigate();
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);

    const loadData = () => {
        setLoading(true);
        apiGet(`/PengajuanApi?unitId=${user?.unitId}&roleId=${user?.roleId}`)
            .then(res => { if (res) setData(res); })
            .finally(() => setLoading(false));
    };

    useEffect(() => { loadData(); }, [user]);

    const handleDelete = async (id) => {
        if (!confirm('Yakin ingin menghapus pengajuan ini beserta semua datanya?')) return;
        const res = await apiDelete(`/PengajuanApi/${id}`);
        if (res) loadData();
    };

    const handleTimBmnView = async (item) => {
        const s = (item.status || '').toLowerCase();

        if (s === 'review' && item.reviewedById !== user?.userId) {
            alert(`Pengajuan ini sedang direview oleh ${item.reviewedByName || 'Tim BMN lain'}. Anda tidak bisa melihat detail saat ini.`);
            return;
        }

        if (s === 'menunggu tim bmn') {
            await apiPatch(`/PengajuanApi/${item.idPengajuan}/status`, {
                status: 'Review',
                userId: user.userId,
                roleId: user.roleId
            });
        }

        navigate(`/pengajuan/${item.idPengajuan}`);
    };

    const renderStatus = (item) => {
        const s = (item.status || '').toLowerCase();
        const statusMap = {
            'draft': { label: 'Draft', bg: '#ffc107', color: '#000' },
            'menunggu pimpinan unit': { label: 'Menunggu Pimpinan Unit', bg: '#fd7e14', color: '#fff' },
            'menunggu wr bpku': { label: 'Menunggu WR BPKU', bg: '#e83e8c', color: '#fff' },
            'menunggu kabiro bpku': { label: 'Menunggu Kabiro BPKU', bg: '#6610f2', color: '#fff' },
            'menunggu tim bmn': { label: 'Menunggu Tim BMN', bg: '#20c997', color: '#fff' },
            'review': { label: 'Review', bg: '#17a2b8', color: '#fff' },
            'reviewed': { label: 'Reviewed', bg: '#6f42c1', color: '#fff' },
            'menunggu kabag umum': { label: 'Menunggu Kabag Umum', bg: '#007bff', color: '#fff' },
            'selesai': { label: 'Selesai', bg: '#28a745', color: '#fff' },
        };
        const cfg = statusMap[s] || { label: item.status, bg: '#6c757d', color: '#fff' };
        return (
            <span className="badge-status" style={{ backgroundColor: cfg.bg, color: cfg.color }}
                title={s === 'review' && item.reviewedByName ? `Direview oleh: ${item.reviewedByName}` : ''}>
                {cfg.label}
            </span>
        );
    };

    const canCreate = user?.roleId === 1;
    const isTimBmn = user?.roleId === 4;

    return (
        <div className="fade-in">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h2 style={{ fontSize: '1.3rem', fontWeight: 700 }}>Daftar Pengajuan Barang Modal</h2>
                    <p style={{ color: 'var(--text-secondary)', fontSize: '0.875rem' }}>
                        {canCreate
                            ? 'Kelola semua pengajuan barang modal unit kerja Anda'
                            : 'Lihat dan kelola pengajuan dari seluruh unit kerja'}
                    </p>
                </div>
                {canCreate && (
                    <Link to="/pengajuan/create" className="btn-primary-custom">
                        <i className="fas fa-plus-circle"></i> Buat Pengajuan Baru
                    </Link>
                )}
            </div>

            {loading ? (
                <div className="text-center py-5"><i className="fas fa-spinner fa-spin fa-2x"></i></div>
            ) : data.length > 0 ? (
                <div className="table-container">
                    <table className="table">
                        <thead>
                            <tr>
                                <th>No</th><th>Tanggal Pengajuan</th><th>Unit Kerja</th><th>Jenis</th>
                                <th>Jumlah Item</th><th>Total Harga</th><th>Status</th><th>Aksi</th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.map((item, i) => {
                                const s = (item.status || '').toLowerCase();
                                const isLockedByOther = isTimBmn && s === 'review' && item.reviewedById !== user?.userId;

                                return (
                                    <tr key={item.idPengajuan} style={isLockedByOther ? { opacity: 0.6 } : {}}>
                                        <td>{i + 1}</td>
                                        <td>{formatDate(item.tanggalPengajuan)}</td>
                                        <td>{item.unitName}</td>
                                        <td>{item.jenisPengajuan || 'Belanja Modal'}</td>
                                        <td><span className="badge bg-secondary">{item.detailCount} item</span></td>
                                        <td className="currency">{formatRupiah(item.totalHarga)}</td>
                                        <td>{renderStatus(item)}</td>
                                        <td>
                                            <div className="d-flex gap-1">
                                                {isTimBmn ? (
                                                    <button
                                                        className="btn-sm-action btn-edit"
                                                        title={isLockedByOther ? `Sedang di-review oleh ${item.reviewedByName}` : 'Lihat Detail'}
                                                        onClick={() => handleTimBmnView(item)}
                                                        disabled={isLockedByOther}
                                                        style={isLockedByOther ? { cursor: 'not-allowed', opacity: 0.5 } : {}}>
                                                        <i className={`fas ${isLockedByOther ? 'fa-lock' : 'fa-eye'}`}></i>
                                                    </button>
                                                ) : (
                                                    <Link to={`/pengajuan/${item.idPengajuan}`} className="btn-sm-action btn-edit" title="Lihat Detail">
                                                        <i className="fas fa-eye"></i>
                                                    </Link>
                                                )}
                                                {s === 'draft' && canCreate && (
                                                    <>
                                                        <Link to={`/pengajuan/edit/${item.idPengajuan}`} className="btn-sm-action btn-edit" title="Edit">
                                                            <i className="fas fa-pencil"></i>
                                                        </Link>
                                                        <button className="btn-sm-action btn-delete" title="Hapus" onClick={() => handleDelete(item.idPengajuan)}>
                                                            <i className="fas fa-trash"></i>
                                                        </button>
                                                    </>
                                                )}
                                            </div>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
            ) : (
                <div className="table-container">
                    <div className="empty-state">
                        <i className="fas fa-folder-open"></i>
                        <h3>Belum Ada Pengajuan</h3>
                        <p>{canCreate ? 'Anda belum membuat pengajuan barang modal.' : 'Belum ada pengajuan dari unit kerja manapun.'}</p>
                        {canCreate && (
                            <Link to="/pengajuan/create" className="btn-primary-custom">
                                <i className="fas fa-plus-circle"></i> Buat Pengajuan Pertama
                            </Link>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
}
