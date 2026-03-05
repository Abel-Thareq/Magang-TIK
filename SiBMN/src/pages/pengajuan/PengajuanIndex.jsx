import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { apiGet, apiDelete, formatRupiah, formatDate } from '../../api/api';

export default function PengajuanIndex() {
    const { user } = useAuth();
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

    return (
        <div className="fade-in">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h2 style={{ fontSize: '1.3rem', fontWeight: 700 }}>Daftar Pengajuan Barang Modal</h2>
                    <p style={{ color: 'var(--text-secondary)', fontSize: '0.875rem' }}>
                        Kelola semua pengajuan barang modal unit kerja Anda
                    </p>
                </div>
                <Link to="/pengajuan/create" className="btn-primary-custom">
                    <i className="fas fa-plus-circle"></i> Buat Pengajuan Baru
                </Link>
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
                            {data.map((item, i) => (
                                <tr key={item.idPengajuan}>
                                    <td>{i + 1}</td>
                                    <td>{formatDate(item.tanggalPengajuan)}</td>
                                    <td>{item.unitName}</td>
                                    <td>{item.jenisPengajuan || 'Belanja Modal'}</td>
                                    <td><span className="badge bg-secondary">{item.detailCount} item</span></td>
                                    <td className="currency">{formatRupiah(item.totalHarga)}</td>
                                    <td>
                                        {item.status === 'draft' && <span className="badge-status badge-draft">Draft</span>}
                                        {item.status === 'approved' && <span className="badge-status badge-approved">Diajukan</span>}
                                        {item.status !== 'draft' && item.status !== 'approved' && <span className="badge-status badge-disapproved">Ditolak</span>}
                                    </td>
                                    <td>
                                        <div className="d-flex gap-1">
                                            <Link to={`/pengajuan/${item.idPengajuan}`} className="btn-sm-action btn-edit" title="Lihat Detail">
                                                <i className="fas fa-eye"></i>
                                            </Link>
                                            {item.status === 'draft' && (
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
                            ))}
                        </tbody>
                    </table>
                </div>
            ) : (
                <div className="table-container">
                    <div className="empty-state">
                        <i className="fas fa-folder-open"></i>
                        <h3>Belum Ada Pengajuan</h3>
                        <p>Anda belum membuat pengajuan barang modal. Klik tombol di bawah untuk membuat pengajuan baru.</p>
                        <Link to="/pengajuan/create" className="btn-primary-custom">
                            <i className="fas fa-plus-circle"></i> Buat Pengajuan Pertama
                        </Link>
                    </div>
                </div>
            )}
        </div>
    );
}
