import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { apiGet, apiPost, apiPatch, apiDelete, formatRupiah, formatDate } from '../../api/api';

// === Progress Tracker Component (Snake / Serpentine Layout) ===
function ProgressTracker({ pengajuan }) {
    const status = (pengajuan.status || '').toLowerCase();

    const stages = [
        { key: 'operator', label: 'Operator Unit Kerja', date: pengajuan.submittedAt },
        { key: 'pimpinan_unit', label: 'Pimpinan Unit Kerja', date: pengajuan.pimpinanUnitApprovedAt },
        { key: 'wr_bpku', label: 'WR BPKU', date: pengajuan.wrBpkuApprovedAt },
        { key: 'kabiro_bpku', label: 'Kabiro BPKU', date: pengajuan.kabiroBpkuApprovedAt },
        { key: 'tim_bmn', label: 'Tim Kerja BMN', date: pengajuan.approvedAt || pengajuan.reviewedAt },
        { key: 'kabag_umum', label: 'Kabag Umum', date: pengajuan.kabagUmumApprovedAt },
    ];

    const row1 = stages.slice(0, 5); // L→R
    const row2 = stages.slice(5);     // R→L (displayed right-aligned)

    const statusToStageIndex = {
        'draft': -1,
        'menunggu pimpinan unit': 0,
        'menunggu wr bpku': 1,
        'menunggu kabiro bpku': 2,
        'menunggu tim bmn': 3,
        'review': 4, 'reviewed': 4, 'menunggu kabag umum': 4,
        'selesai': 5,
    };

    const currentIndex = statusToStageIndex[status] ?? -1;
    const isInTimBmn = ['review', 'reviewed', 'menunggu kabag umum'].includes(status);
    const timBmnCompleted = status === 'menunggu kabag umum' || status === 'selesai';

    const getStageStatus = (idx) => {
        if (idx === 4) {
            if (timBmnCompleted || status === 'selesai') return 'completed';
            if (isInTimBmn) return 'active';
            if (currentIndex === 3) return 'current';
            return 'upcoming';
        }
        if (idx < currentIndex) return 'completed';
        if (idx === currentIndex) return 'current';
        if (idx === 5 && status === 'selesai') return 'completed';
        return 'upcoming';
    };

    const getWaitingText = () => {
        const map = {
            'menunggu pimpinan unit': 'Sedang menunggu persetujuan Pimpinan Unit Kerja',
            'menunggu wr bpku': 'Sedang menunggu persetujuan WR BPKU',
            'menunggu kabiro bpku': 'Sedang menunggu persetujuan Kabiro BPKU',
            'menunggu tim bmn': 'Sedang menunggu review Tim BMN',
            'review': 'Sedang direview Tim BMN',
            'reviewed': 'Sedang menunggu persetujuan Pimpinan Tim BMN',
            'menunggu kabag umum': 'Sedang menunggu persetujuan Kabag Umum',
            'selesai': 'Pengajuan telah selesai',
        };
        return map[status] || '';
    };

    const isNodeActive = (ss) => ss === 'completed' || ss === 'current' || ss === 'active';
    const nodeColor = (ss) => isNodeActive(ss) ? '#f0ad4e' : '#dee2e6';
    const lineActive = (fromIdx, toIdx) => {
        return isNodeActive(getStageStatus(fromIdx)) && getStageStatus(toIdx) !== 'upcoming';
    };
    const lineColor = (fromIdx, toIdx) => lineActive(fromIdx, toIdx) ? '#f0ad4e' : '#dee2e6';

    const renderCircle = (ss) => (
        <div style={{
            width: 26, height: 26, borderRadius: '50%', flexShrink: 0,
            backgroundColor: nodeColor(ss), border: `3px solid ${nodeColor(ss)}`,
            display: 'flex', alignItems: 'center', justifyContent: 'center',
        }}>
            {ss === 'completed' && <i className="fas fa-check" style={{ color: '#fff', fontSize: '0.6rem' }}></i>}
            {(ss === 'current' || ss === 'active') && <div style={{ width: 9, height: 9, borderRadius: '50%', backgroundColor: '#fff' }}></div>}
        </div>
    );

    // Curve connector goes from last node of row1 (Tim BMN, idx 4) down to first of row2 (Kabag Umum, idx 5)
    const curveActive = lineActive(4, 5);
    const curveCol = curveActive ? '#f0ad4e' : '#dee2e6';

    return (
        <div className="info-card" style={{ backgroundColor: '#fffbea' }}>
            <div className="info-card-title" style={{ color: '#856404' }}>
                <i className="fas fa-route"></i> Informasi Posisi Pengajuan
            </div>

            <div style={{ padding: '20px 20px 10px', overflowX: 'auto' }}>
                <div style={{ minWidth: '700px', paddingRight: '50px' }}>

                    {/* === Single flex row: first 4 nodes + last block (Tim BMN + curve + Kabag Umum) === */}
                    <div style={{ display: 'flex', alignItems: 'flex-start' }}>
                        {/* First 4 nodes with horizontal lines */}
                        {row1.slice(0, 4).map((stage, i) => {
                            const ss = getStageStatus(i);
                            return (
                                <div key={stage.key} style={{ display: 'flex', alignItems: 'flex-start', flex: '1 1 0' }}>
                                    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '110px' }}>
                                        <div style={{ fontSize: '0.68rem', color: isNodeActive(ss) ? '#6c757d' : '#ccc', height: 16, textAlign: 'center', marginBottom: 4 }}>
                                            {stage.date ? formatDate(stage.date) : '\u00A0'}
                                        </div>
                                        {renderCircle(ss)}
                                        <div style={{ fontSize: '0.72rem', fontWeight: isNodeActive(ss) ? 600 : 400, color: isNodeActive(ss) ? '#333' : '#adb5bd', marginTop: 5, textAlign: 'center', lineHeight: 1.2 }}>
                                            {stage.label}
                                        </div>
                                        {ss === 'current' && status !== 'selesai' && (
                                            <div style={{ fontSize: '0.58rem', color: '#fd7e14', marginTop: 2, textAlign: 'center', fontStyle: 'italic' }}>
                                                Sedang menunggu persetujuan
                                            </div>
                                        )}
                                    </div>
                                    {/* Horizontal line */}
                                    <div style={{ flex: 1, height: 3, marginTop: 33, backgroundColor: lineColor(i, i + 1), borderRadius: 2, minWidth: 12 }}></div>
                                </div>
                            );
                        })}

                        {/* === LAST BLOCK: Tim BMN + Curve + Kabag Umum (all in one column) === */}
                        <div style={{ flex: '0 0 auto', overflow: 'visible' }}>
                            {/* Tim BMN node */}
                            {(() => {
                                const ss = getStageStatus(4);
                                return (
                                    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '110px' }}>
                                        <div style={{ fontSize: '0.68rem', color: isNodeActive(ss) ? '#6c757d' : '#ccc', height: 16, textAlign: 'center', marginBottom: 4 }}>
                                            {stages[4].date ? formatDate(stages[4].date) : '\u00A0'}
                                        </div>
                                        {renderCircle(ss)}
                                        <div style={{ fontSize: '0.72rem', fontWeight: isNodeActive(ss) ? 600 : 400, color: isNodeActive(ss) ? '#333' : '#adb5bd', marginTop: 5, textAlign: 'center', lineHeight: 1.2 }}>
                                            {stages[4].label}
                                        </div>
                                    </div>
                                );
                            })()}

                            {/* SVG Curve: from Tim BMN circle center down to Kabag Umum circle center */}
                            {/* The curve starts at center (55px), goes right, then comes back to center */}
                            <svg width="110" height="70" viewBox="0 0 110 70" style={{ overflow: 'visible', display: 'block', margin: '0 auto' }}>
                                <path
                                    d="M 115 -28 C 185 -28, 185 85, 115 85"
                                    stroke={curveCol}
                                    strokeWidth="3"
                                    fill="none"
                                    strokeLinecap="round"
                                />
                            </svg>

                            {/* Kabag Umum node */}
                            {(() => {
                                const ss = getStageStatus(5);
                                return (
                                    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '110px' }}>
                                        {renderCircle(ss)}
                                        <div style={{ fontSize: '0.72rem', fontWeight: isNodeActive(ss) ? 600 : 400, color: isNodeActive(ss) ? '#333' : '#adb5bd', marginTop: 5, textAlign: 'center', lineHeight: 1.2 }}>
                                            {stages[5].label}
                                        </div>
                                        {stages[5].date && (
                                            <div style={{ fontSize: '0.68rem', color: '#6c757d', marginTop: 2, textAlign: 'center' }}>
                                                {formatDate(stages[5].date)}
                                            </div>
                                        )}
                                        {ss === 'current' && status !== 'selesai' && (
                                            <div style={{ fontSize: '0.58rem', color: '#fd7e14', marginTop: 2, textAlign: 'center', fontStyle: 'italic' }}>
                                                Sedang menunggu persetujuan
                                            </div>
                                        )}
                                    </div>
                                );
                            })()}
                        </div>
                    </div>

                </div>

                {/* Waiting text */}
                {getWaitingText() && (
                    <div style={{ textAlign: 'center', marginTop: 16, fontSize: '0.82rem', color: '#856404', fontWeight: 500 }}>
                        <i className="fas fa-clock me-1"></i>
                        {getWaitingText()}
                    </div>
                )}
            </div>
        </div>
    );
}

// === Main Component ===
export default function PengajuanDetails() {
    const { id } = useParams();
    const { user } = useAuth();
    const [pengajuan, setPengajuan] = useState(null);
    const [details, setDetails] = useState([]);
    const [loading, setLoading] = useState(true);
    const [msg, setMsg] = useState('');

    const loadData = () => {
        setLoading(true);
        apiGet(`/PengajuanApi/${id}?roleId=${user?.roleId || ''}`).then(data => {
            if (data) {
                setPengajuan(data.pengajuan);
                setDetails(data.details);
            }
        }).finally(() => setLoading(false));
    };

    useEffect(() => { loadData(); }, [id]);

    // Operator: submit draft
    const handleSubmit = async () => {
        if (!confirm('Yakin ingin mengajukan pengajuan ini?')) return;
        const res = await apiPost(`/PengajuanApi/${id}/submit`);
        if (res) { setMsg(res.message); loadData(); }
    };

    // Generic approve/reject handler
    const handleStatusChange = async (status, confirmMsg) => {
        if (!confirm(confirmMsg)) return;
        const res = await apiPatch(`/PengajuanApi/${id}/status`, {
            status,
            userId: user.userId,
            roleId: user.roleId
        });
        if (res) { setMsg(res.message || 'Status berhasil diperbarui'); loadData(); }
    };

    // Tim BMN: finish review
    const handleFinishReview = () => handleStatusChange('Reviewed', 'Tandai review selesai?');

    // Reviewer: toggle exclude
    const handleToggleExclude = async (detailId) => {
        const res = await apiPatch(`/DetailPengajuanApi/${detailId}/toggle-exclude`);
        if (res) { loadData(); }
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

    const statusLower = (pengajuan.status || '').toLowerCase();
    const isTimBmn = user?.roleId === 4;
    const isPimpinanBmn = user?.roleId === 5;
    const isPimpinanUnit = user?.roleId === 6;
    const isWrBpku = user?.roleId === 7;
    const isKabiroBpku = user?.roleId === 8;
    const isKabagUmum = user?.roleId === 9;
    const isReviewer = isTimBmn && pengajuan.reviewedById === user?.userId;
    const isAdminUnit = user?.roleId === 1;

    // Admin can edit during draft
    const canAdminEdit = isAdminUnit && statusLower === 'draft';
    // Reviewer can edit during review
    const canReviewerEdit = isReviewer && statusLower === 'review';
    const canEdit = canAdminEdit || canReviewerEdit;

    // Status badge rendering
    const statusBadge = () => {
        const map = {
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
        const cfg = map[statusLower] || { label: pengajuan.status, bg: '#6c757d', color: '#fff' };
        return <span className="badge" style={{ backgroundColor: cfg.bg, color: cfg.color }}>{cfg.label}</span>;
    };

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
                            ID: #{pengajuan.idPengajuan} | {statusBadge()}
                        </span>
                        {/* Reviewer info visible only to Tim BMN and Pimpinan BMN */}
                        {(isTimBmn || isPimpinanBmn) && statusLower === 'review' && pengajuan.reviewedByName && (
                            <div style={{ fontSize: '0.8rem', marginTop: 4, color: '#17a2b8' }}>
                                <i className="fas fa-user-check me-1"></i>Sedang direview oleh: <strong>{pengajuan.reviewedByName}</strong>
                            </div>
                        )}
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
                {canAdminEdit && (
                    <div className="mt-3">
                        <Link to={`/pengajuan/edit/${pengajuan.idPengajuan}`} className="btn-sm-action btn-edit">
                            <i className="fas fa-pencil"></i> Edit Data Surat
                        </Link>
                    </div>
                )}
            </div>

            {/* Progress Tracker */}
            {statusLower !== 'draft' && (
                <ProgressTracker pengajuan={pengajuan} />
            )}

            {/* Informasi Review - hanya terlihat oleh Tim BMN dan Pimpinan BMN */}
            {(isTimBmn || isPimpinanBmn) && pengajuan.reviewedByName && (
                <div className="info-card">
                    <div className="info-card-title"><i className="fas fa-clipboard-check"></i> Informasi Review</div>
                    <div className="info-grid">
                        <div className="info-item">
                            <label>Direview oleh</label>
                            <span><i className="fas fa-user-check me-1" style={{ color: '#17a2b8' }}></i>{pengajuan.reviewedByName}</span>
                        </div>
                        <div className="info-item">
                            <label>Tanggal Review</label>
                            <span><i className="fas fa-calendar-check me-1" style={{ color: '#17a2b8' }}></i>{pengajuan.reviewedAt ? formatDate(pengajuan.reviewedAt, 'long') : '-'}</span>
                        </div>
                        {pengajuan.approvedByName && (
                            <>
                                <div className="info-item">
                                    <label>Disetujui oleh</label>
                                    <span><i className="fas fa-user-shield me-1" style={{ color: '#28a745' }}></i>{pengajuan.approvedByName}</span>
                                </div>
                                <div className="info-item">
                                    <label>Tanggal Persetujuan</label>
                                    <span><i className="fas fa-calendar-check me-1" style={{ color: '#28a745' }}></i>{pengajuan.approvedAt ? formatDate(pengajuan.approvedAt, 'long') : '-'}</span>
                                </div>
                            </>
                        )}
                    </div>
                </div>
            )}

            {/* Daftar Barang */}
            <div className="table-container">
                <div className="table-header">
                    <div className="table-title"><i className="fas fa-boxes-stacked me-2"></i>Daftar Barang Modal</div>
                    {canAdminEdit && (
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
                                    {canEdit && <th style={{ width: 150 }}>Aksi</th>}
                                </tr>
                            </thead>
                            <tbody>
                                {details.map(item => {
                                    const excluded = item.isExcluded;
                                    const rowStyle = excluded
                                        ? { opacity: 0.4, backgroundColor: '#f0f0f0', textDecoration: 'line-through', color: '#999' }
                                        : {};

                                    return (
                                        <tr key={item.idDetPengajuan} style={rowStyle}>
                                            <td><span className="badge bg-secondary">{item.noPrioritas}</span></td>
                                            <td><strong>{item.barangNama}</strong><br /><small className="text-muted">{item.barangKode}</small></td>
                                            <td style={{ maxWidth: 200, fontSize: '0.82rem', color: excluded ? '#999' : 'var(--text-secondary)' }}>{item.spesifikasi || '-'}</td>
                                            <td>{item.jumlahDiminta}</td>
                                            <td>Unit</td>
                                            <td className="currency">{formatRupiah(item.hargaSatuan)}</td>
                                            <td className="currency" style={{ color: excluded ? '#999' : 'var(--primary)', fontWeight: 700 }}>{formatRupiah(item.jumlahHarga)}</td>
                                            <td style={{ fontSize: '0.82rem' }}>{item.gedungNama}<br /><small className="text-muted">{item.ruangNama}</small></td>
                                            <td>
                                                {item.asalBarang === 'Import' ? <span className="badge bg-info">Import</span> : <span className="badge bg-success">PDN</span>}
                                            </td>
                                            {canEdit && (
                                                <td>
                                                    <div className="d-flex gap-1 flex-wrap">
                                                        <button className="btn-sm-action btn-move" title="Naikkan Prioritas" onClick={() => handleMove(item.idDetPengajuan, 'moveup')}><i className="fas fa-arrow-up"></i></button>
                                                        <button className="btn-sm-action btn-move" title="Turunkan Prioritas" onClick={() => handleMove(item.idDetPengajuan, 'movedown')}><i className="fas fa-arrow-down"></i></button>
                                                        {canAdminEdit && (
                                                            <>
                                                                <Link to={`/detailpengajuan/edit/${item.idDetPengajuan}`} className="btn-sm-action btn-edit" title="Edit"><i className="fas fa-pencil"></i></Link>
                                                                <button className="btn-sm-action btn-delete" title="Hapus" onClick={() => handleDeleteDetail(item.idDetPengajuan)}><i className="fas fa-trash"></i></button>
                                                            </>
                                                        )}
                                                        {canReviewerEdit && (
                                                            <button
                                                                className={`btn-sm-action ${excluded ? 'btn-edit' : 'btn-delete'}`}
                                                                title={excluded ? 'Kembalikan barang' : 'Tandai tidak diperlukan'}
                                                                onClick={() => handleToggleExclude(item.idDetPengajuan)}
                                                                style={excluded ? { backgroundColor: '#28a745', borderColor: '#28a745', color: '#fff' } : {}}>
                                                                <i className={`fas ${excluded ? 'fa-undo' : 'fa-ban'}`}></i>
                                                            </button>
                                                        )}
                                                    </div>
                                                </td>
                                            )}
                                        </tr>
                                    );
                                })}
                            </tbody>
                            <tfoot>
                                <tr className="total-row">
                                    <td colSpan="6" style={{ textAlign: 'right', fontSize: '1rem' }}><strong>TOTAL</strong></td>
                                    <td className="currency" style={{ fontSize: '1.05rem' }}>{formatRupiah(pengajuan.totalHarga)}</td>
                                    <td colSpan={canEdit ? 3 : 2}></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                ) : (
                    <div className="empty-state">
                        <i className="fas fa-box-open"></i>
                        <h3>Belum Ada Barang</h3>
                        <p>Belum ada barang yang ditambahkan ke pengajuan ini.</p>
                        {canAdminEdit && (
                            <Link to={`/detailpengajuan/create/${pengajuan.idPengajuan}`} className="btn-primary-custom">
                                <i className="fas fa-plus-circle"></i> Tambah Barang Pertama
                            </Link>
                        )}
                    </div>
                )}
            </div>

            {/* === Action Buttons === */}

            {/* Admin Unit Kerja: submit draft */}
            {canAdminEdit && details.length > 0 && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem' }} onClick={handleSubmit}>
                        <i className="fas fa-paper-plane"></i> Ajukan Pengajuan
                    </button>
                </div>
            )}

            {/* Pimpinan Unit Kerja: approve/reject */}
            {isPimpinanUnit && statusLower === 'menunggu pimpinan unit' && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#28a745', borderColor: '#28a745' }}
                        onClick={() => handleStatusChange('ApprovePimpinanUnit', 'Yakin ingin menyetujui pengajuan ini?')}>
                        <i className="fas fa-check-circle"></i> Setujui
                    </button>
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#dc3545', borderColor: '#dc3545' }}
                        onClick={() => handleStatusChange('RejectPimpinanUnit', 'Yakin ingin menolak pengajuan ini? Status akan kembali ke Draft.')}>
                        <i className="fas fa-times-circle"></i> Tolak
                    </button>
                </div>
            )}

            {/* WR BPKU: approve/reject */}
            {isWrBpku && statusLower === 'menunggu wr bpku' && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#28a745', borderColor: '#28a745' }}
                        onClick={() => handleStatusChange('ApproveWrBpku', 'Yakin ingin menyetujui pengajuan ini?')}>
                        <i className="fas fa-check-circle"></i> Setujui
                    </button>
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#dc3545', borderColor: '#dc3545' }}
                        onClick={() => handleStatusChange('RejectWrBpku', 'Yakin ingin menolak pengajuan ini?')}>
                        <i className="fas fa-times-circle"></i> Tolak
                    </button>
                </div>
            )}

            {/* Kabiro BPKU: approve/reject */}
            {isKabiroBpku && statusLower === 'menunggu kabiro bpku' && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#28a745', borderColor: '#28a745' }}
                        onClick={() => handleStatusChange('ApproveKabiroBpku', 'Yakin ingin menyetujui pengajuan ini?')}>
                        <i className="fas fa-check-circle"></i> Setujui
                    </button>
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#dc3545', borderColor: '#dc3545' }}
                        onClick={() => handleStatusChange('RejectKabiroBpku', 'Yakin ingin menolak pengajuan ini?')}>
                        <i className="fas fa-times-circle"></i> Tolak
                    </button>
                </div>
            )}

            {/* Tim BMN: finish review */}
            {isReviewer && statusLower === 'review' && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#6f42c1', borderColor: '#6f42c1' }} onClick={handleFinishReview}>
                        <i className="fas fa-check-double"></i> Selesai Review
                    </button>
                </div>
            )}

            {/* Pimpinan BMN: approve or reject */}
            {isPimpinanBmn && statusLower === 'reviewed' && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#28a745', borderColor: '#28a745' }}
                        onClick={() => handleStatusChange('Approve', 'Yakin ingin menyetujui pengajuan ini?')}>
                        <i className="fas fa-check-circle"></i> Setujui Pengajuan
                    </button>
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#dc3545', borderColor: '#dc3545' }}
                        onClick={() => handleStatusChange('Reject', 'Yakin ingin menolak pengajuan ini?')}>
                        <i className="fas fa-times-circle"></i> Tolak Pengajuan
                    </button>
                </div>
            )}

            {/* Kabag Umum: approve/reject */}
            {isKabagUmum && statusLower === 'menunggu kabag umum' && (
                <div className="d-flex gap-3 mt-4">
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#28a745', borderColor: '#28a745' }}
                        onClick={() => handleStatusChange('ApproveKabagUmum', 'Yakin ingin menyetujui pengajuan ini?')}>
                        <i className="fas fa-check-circle"></i> Setujui
                    </button>
                    <button className="btn-primary-custom" style={{ padding: '0.75rem 2rem', backgroundColor: '#dc3545', borderColor: '#dc3545' }}
                        onClick={() => handleStatusChange('RejectKabagUmum', 'Yakin ingin menolak pengajuan ini?')}>
                        <i className="fas fa-times-circle"></i> Tolak
                    </button>
                </div>
            )}
        </div>
    );
}
