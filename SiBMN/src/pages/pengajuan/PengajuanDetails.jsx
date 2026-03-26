import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { apiGet, apiPost, apiPatch, apiDelete, formatRupiah, formatDate } from '../../api/api';

// === Progress Tracker Component (Snake / Serpentine Layout) ===
function ProgressTracker({ pengajuan, roleId }) {
    const status = (pengajuan.status || '').toLowerCase();
    const isBmnRole = roleId === 4 || roleId === 5; // Tim BMN or Pimpinan Tim BMN

    // Build stages dynamically based on role
    const stages = [];
    stages.push({ key: 'operator', label: 'Operator Unit Kerja', date: pengajuan.submittedAt });
    stages.push({ key: 'pimpinan_unit', label: 'Pimpinan Unit Kerja', date: pengajuan.pimpinanUnitApprovedAt });
    stages.push({ key: 'wr_bpku', label: 'WR BPKU', date: pengajuan.wrBpkuApprovedAt });
    stages.push({ key: 'kabiro_bpku', label: 'Kabiro BPKU', date: pengajuan.kabiroBpkuApprovedAt });
    stages.push({ key: 'tim_bmn', label: 'Tim Kerja BMN', date: pengajuan.reviewedAt });
    if (isBmnRole) {
        stages.push({ key: 'pimpinan_bmn', label: 'Pimpinan Tim BMN', date: pengajuan.approvedAt });
    }
    stages.push({ key: 'kabag_umum', label: 'Kabag Umum', date: pengajuan.kabagUmumApprovedAt });

    // Index of Tim BMN in stages = 4
    // Index of Pimpinan BMN (if BMN role) = 5
    // Index of Kabag Umum = isBmnRole ? 6 : 5
    const timBmnIdx = 4;
    const pimpinanBmnIdx = isBmnRole ? 5 : -1;
    const kabagIdx = isBmnRole ? 6 : 5;
    const lastStageIdx = stages.length - 1;

    // Row 1: first 5 nodes (up to Tim BMN), Row 2: remaining (after Tim BMN)
    const row1 = stages.slice(0, 5);
    const row2 = stages.slice(5); // 1 item (non-BMN) or 2 items (BMN)

    const isInTimBmn = ['review', 'reviewed', 'menunggu kabag umum'].includes(status);
    const timBmnCompleted = status === 'menunggu kabag umum' || status === 'selesai';

    const getStageStatus = (idx) => {
        // Tim Kerja BMN (idx 4)
        if (idx === timBmnIdx) {
            if (isBmnRole) {
                // For BMN roles: Tim BMN is completed when status reaches reviewed or beyond
                if (['reviewed', 'menunggu kabag umum', 'selesai'].includes(status)) return 'completed';
                if (status === 'review') return 'active';
                if (status === 'menunggu tim bmn') return 'current';
                return idx <= 3 ? 'upcoming' : 'upcoming';
            } else {
                // For non-BMN: Tim BMN covers review+reviewed combined
                if (timBmnCompleted || status === 'selesai') return 'completed';
                if (isInTimBmn) return 'active';
                if (status === 'menunggu tim bmn') return 'current';
                return 'upcoming';
            }
        }
        // Pimpinan Tim BMN (idx 5, only for BMN roles)
        if (isBmnRole && idx === pimpinanBmnIdx) {
            if (['menunggu kabag umum', 'selesai'].includes(status)) return 'completed';
            if (status === 'reviewed') return 'current';
            if (['review', 'menunggu tim bmn'].includes(status)) return 'upcoming';
            // Check if we're past it
            return 'upcoming';
        }
        // Kabag Umum
        if (idx === kabagIdx) {
            if (status === 'selesai') return 'completed';
            if (status === 'menunggu kabag umum') return 'current';
            return 'upcoming';
        }
        // General stages (0-3)
        const stageOrder = {
            'draft': -1,
            'menunggu pimpinan unit': 0,
            'menunggu wr bpku': 1,
            'menunggu kabiro bpku': 2,
            'menunggu tim bmn': 3,
            'review': 4, 'reviewed': 5, 'menunggu kabag umum': 6,
            'selesai': 7,
        };
        const currentOrder = stageOrder[status] ?? -1;
        const stageOrderVal = stageOrder[Object.keys(stageOrder).find(
            (_, si) => si === idx
        )] ?? idx;
        if (idx < 4) {
            // Simple: compare idx to where we are
            const statusProgress = {
                'draft': -1,
                'menunggu pimpinan unit': 0,
                'menunggu wr bpku': 1,
                'menunggu kabiro bpku': 2,
                'menunggu tim bmn': 3,
                'review': 4, 'reviewed': 4, 'menunggu kabag umum': 5,
                'selesai': 6,
            };
            const progress = statusProgress[status] ?? -1;
            if (idx < progress) return 'completed';
            if (idx === progress) return 'current';
            return 'upcoming';
        }
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
    const lineColor = (fromIdx, toIdx) => {
        return (isNodeActive(getStageStatus(fromIdx)) && getStageStatus(toIdx) !== 'upcoming') ? '#f0ad4e' : '#dee2e6';
    };

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

    const renderNode = (stage, stageIdx, showDate = true) => {
        const ss = getStageStatus(stageIdx);
        return (
            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '110px' }}>
                {showDate && (
                    <div style={{ fontSize: '0.68rem', color: isNodeActive(ss) ? '#6c757d' : '#ccc', height: 16, textAlign: 'center', marginBottom: 4 }}>
                        {stage.date ? formatDate(stage.date) : '\u00A0'}
                    </div>
                )}
                {renderCircle(ss)}
                <div style={{ fontSize: '0.72rem', fontWeight: isNodeActive(ss) ? 600 : 400, color: isNodeActive(ss) ? '#333' : '#adb5bd', marginTop: 5, textAlign: 'center', lineHeight: 1.2 }}>
                    {stage.label}
                </div>
                {stage.date && !showDate && (
                    <div style={{ fontSize: '0.68rem', color: '#6c757d', marginTop: 2, textAlign: 'center' }}>
                        {formatDate(stage.date)}
                    </div>
                )}
                {ss === 'current' && status !== 'selesai' && (
                    <div style={{ fontSize: '0.58rem', color: '#fd7e14', marginTop: 2, textAlign: 'center', fontStyle: 'italic' }}>
                        Sedang menunggu persetujuan
                    </div>
                )}
            </div>
        );
    };

    // Curve connects Tim BMN (last row1 node) to first row2 node
    const firstRow2Idx = 5; // index in stages array
    const curveCol = lineColor(timBmnIdx, firstRow2Idx);

    return (
        <div className="info-card" style={{ backgroundColor: '#fffbea' }}>
            <div className="info-card-title" style={{ color: '#856404' }}>
                <i className="fas fa-route"></i> Informasi Posisi Pengajuan
            </div>

            <div style={{ padding: '20px 20px 10px', overflowX: 'auto' }}>
                <div style={{ position: 'relative', minWidth: '700px', paddingRight: '50px' }}>

                    {/* === ROW 1: 5 nodes (L→R) === */}
                    <div style={{ display: 'flex', alignItems: 'flex-start' }}>
                        {row1.slice(0, 4).map((stage, i) => {
                            const ss = getStageStatus(i);
                            return (
                                <div key={stage.key} style={{ display: 'flex', alignItems: 'flex-start', flex: '1 1 0' }}>
                                    {renderNode(stage, i, true)}
                                    <div style={{ flex: 1, height: 3, marginTop: 33, backgroundColor: lineColor(i, i + 1), borderRadius: 2, minWidth: 12 }}></div>
                                </div>
                            );
                        })}
                        {/* Tim BMN (last node) */}
                        <div style={{ flex: '0 0 auto' }}>
                            {renderNode(stages[timBmnIdx], timBmnIdx, true)}
                        </div>
                    </div>

                    {/* === ROW 2: mirrors same flex structure for perfect alignment === */}
                    <div style={{ display: 'flex', alignItems: 'flex-start', marginTop: '55px' }}>
                        {isBmnRole ? (
                            <>
                                {/* 3 invisible spacers matching first 3 flex items of row 1 */}
                                <div style={{ flex: '1 1 0' }} />
                                <div style={{ flex: '1 1 0' }} />
                                <div style={{ flex: '1 1 0' }} />
                                {/* Kabag Umum + line (matches 4th flex item position = Kabiro BPKU) */}
                                <div style={{ display: 'flex', alignItems: 'flex-start', flex: '1 1 0' }}>
                                    {renderNode(stages[kabagIdx], kabagIdx, false)}
                                    <div style={{ flex: 1, height: 3, marginTop: 13, backgroundColor: lineColor(pimpinanBmnIdx, kabagIdx), borderRadius: 2, minWidth: 12 }}></div>
                                </div>
                                {/* Pimpinan BMN (matches Tim BMN position) */}
                                <div style={{ flex: '0 0 auto' }}>
                                    {renderNode(stages[pimpinanBmnIdx], pimpinanBmnIdx, false)}
                                </div>
                            </>
                        ) : (
                            <>
                                {/* 4 invisible spacers matching first 4 flex items of row 1 */}
                                <div style={{ flex: '1 1 0' }} />
                                <div style={{ flex: '1 1 0' }} />
                                <div style={{ flex: '1 1 0' }} />
                                <div style={{ flex: '1 1 0' }} />
                                {/* Kabag Umum (matches Tim BMN position) */}
                                <div style={{ flex: '0 0 auto' }}>
                                    {renderNode(stages[kabagIdx], kabagIdx, false)}
                                </div>
                            </>
                        )}
                    </div>

                    {/* === SVG Curve (absolute): connects Tim BMN → rightmost row2 node === */}
                    <svg
                        style={{
                            position: 'absolute',
                            top: '31px', // Exact center of row 1 circles
                            right: '50px', // Exact alignment with the 110px flex column
                            pointerEvents: 'none',
                            overflow: 'visible',
                        }}
                        width="110"
                        height="115"
                        viewBox="0 0 110 115"
                        fill="none"
                    >
                        <path
                            d="M 110 0 C 170 0, 170 105, 110 105"
                            stroke={curveCol}
                            strokeWidth="3"
                            fill="none"
                            strokeLinecap="round"
                        />
                    </svg>

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
                <ProgressTracker pengajuan={pengajuan} roleId={user?.roleId} />
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
