import { useEffect, useState, useRef, useLayoutEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { apiGet, apiPost, apiPatch, apiDelete, formatRupiah, formatDate } from '../../api/api';

const NODES_PER_ROW = 5;

function ProgressTracker({ pengajuan, roleId }) {
    const status = (pengajuan.status || '').toLowerCase();
    const containerRef = useRef(null);
    const circleRefs = useRef({});
    const [curvePaths, setCurvePaths] = useState([]);

    const allStages = [
        { label: 'Operator Unit Kerja', date: pengajuan.submittedAt },
        { label: 'Pimpinan Unit Kerja', date: pengajuan.pimpinanUnitApprovedAt },
        { label: 'WR BPKU', date: pengajuan.wrBpkuApprovedAt },
        { label: 'Kabiro BPKU', date: pengajuan.kabiroBpkuApprovedAt },
        { label: 'Tim Kerja BMN', date: pengajuan.reviewedAt },
        { label: 'Ketua Tim Kerja BMN', date: pengajuan.approvedAt },
        { label: 'Kabag Umum', date: pengajuan.kabagUmumApprovedAt },
        { label: 'Ketua Tim Bidang Perencanaan Anggaran', date: null },
        { label: 'Kuasa Pengguna Anggaran (KPA)', date: null },
    ];

    const rows = [];
    for (let i = 0; i < allStages.length; i += NODES_PER_ROW) {
        rows.push(allStages.slice(i, Math.min(i + NODES_PER_ROW, allStages.length)));
    }

    const getStatus = (gi) => {
        if (gi === 4) {
            if (['menunggu kabag umum', 'selesai'].includes(status)) return 'completed';
            if (['review', 'reviewed'].includes(status)) return 'active';
            if (status === 'menunggu tim bmn') return 'current';
            return 'upcoming';
        }
        if (gi === 5) {
            if (status === 'selesai' || status === 'menunggu kabag umum') return 'completed';
            if (status === 'reviewed') return 'current';
            return 'upcoming';
        }
        if (gi === 6) {
            if (status === 'selesai') return 'completed';
            if (status === 'menunggu kabag umum') return 'current';
            return 'upcoming';
        }
        if (gi >= 7) return status === 'selesai' ? 'completed' : 'upcoming';
        const p = { 'draft': -1, 'menunggu pimpinan unit': 1, 'menunggu wr bpku': 2, 'menunggu kabiro bpku': 3, 'menunggu tim bmn': 4, 'review': 4, 'reviewed': 4, 'menunggu kabag umum': 5, 'selesai': 6 }[status] ?? -1;
        if (gi < p) return 'completed';
        if (gi === p) return 'current';
        return 'upcoming';
    };

    const isAct = (ss) => ss === 'completed' || ss === 'current' || ss === 'active';
    const nCol = (ss) => isAct(ss) ? '#f0ad4e' : '#dee2e6';
    const lnCol = (a, b) => (isAct(getStatus(a)) && getStatus(b) !== 'upcoming') ? '#f0ad4e' : '#dee2e6';

    const curveColor = (ri) => {
        const lastG = ri * NODES_PER_ROW + rows[ri].length - 1;
        const firstNextG = (ri + 1) * NODES_PER_ROW;
        return firstNextG < allStages.length ? lnCol(lastG, firstNextG) : '#dee2e6';
    };

    const setRef = (key) => (el) => { if (el) circleRefs.current[key] = el; };

    const renderCircle = (ss, refKey) => (
        <div ref={refKey ? setRef(refKey) : null} style={{
            width: 26, height: 26, borderRadius: '50%', flexShrink: 0, zIndex: 2,
            backgroundColor: nCol(ss), border: `3px solid ${nCol(ss)}`,
            display: 'flex', alignItems: 'center', justifyContent: 'center',
        }}>
            {ss === 'completed' && <i className="fas fa-check" style={{ color: '#fff', fontSize: '0.6rem' }}></i>}
            {(ss === 'current' || ss === 'active') && <div style={{ width: 9, height: 9, borderRadius: '50%', backgroundColor: '#fff' }}></div>}
        </div>
    );

    useEffect(() => {
        const raf = requestAnimationFrame(() => {
            if (!containerRef.current) return;
            const cR = containerRef.current.getBoundingClientRect();
            const paths = [];
            for (let r = 0; r < rows.length - 1; r++) {
                const side = r % 2 === 0 ? 'right' : 'left';
                const fromEl = circleRefs.current[`r${r}-${side}`];
                const toEl = circleRefs.current[`r${r + 1}-${side}`];
                if (fromEl && toEl) {
                    const fR = fromEl.getBoundingClientRect();
                    const tR = toEl.getBoundingClientRect();
                    const fX = fR.left + 13 - cR.left, fY = fR.top + 13 - cR.top;
                    const tX = tR.left + 13 - cR.left, tY = tR.top + 13 - cR.top;
                    const arc = 40;
                    const cpX = side === 'right' ? Math.max(fX, tX) + arc : Math.min(fX, tX) - arc;
                    paths.push({ d: `M ${fX} ${fY} C ${cpX} ${fY}, ${cpX} ${tY}, ${tX} ${tY}`, color: curveColor(r) });
                }
            }
            setCurvePaths(paths);
        });
        return () => cancelAnimationFrame(raf);
    }, [status]);

    const getWaitingText = () => ({
        'menunggu pimpinan unit': 'Sedang menunggu persetujuan Pimpinan Unit Kerja',
        'menunggu wr bpku': 'Sedang menunggu persetujuan WR BPKU',
        'menunggu kabiro bpku': 'Sedang menunggu persetujuan Kabiro BPKU',
        'menunggu tim bmn': 'Sedang menunggu review Tim BMN',
        'review': 'Sedang direview Tim BMN',
        'reviewed': 'Sedang menunggu persetujuan Ketua Tim Kerja BMN',
        'menunggu kabag umum': 'Sedang menunggu persetujuan Kabag Umum',
        'selesai': 'Pengajuan telah selesai',
    }[status] || '');

    return (
        <div className="info-card" style={{ backgroundColor: '#fffbea' }}>
            <div className="info-card-title" style={{ color: '#856404' }}>
                <i className="fas fa-route"></i> Informasi Posisi Pengajuan
            </div>
            <div style={{ padding: '20px 20px 10px', overflowX: 'auto' }}>
                <div ref={containerRef} style={{ position: 'relative', minWidth: '700px' }}>
                    {rows.map((row, ri) => {
                        const isLR = ri % 2 === 0;
                        const display = isLR ? row : [...row].reverse();
                        const gOff = ri * NODES_PER_ROW;
                        return (
                            <div key={ri} style={{ display: 'flex', alignItems: 'center', padding: ri === 0 ? '24px 55px 55px' : '20px 55px 55px', marginTop: ri > 0 ? '10px' : 0 }}>
                                {display.map((stage, di) => {
                                    const gi = isLR ? gOff + di : gOff + (row.length - 1 - di);
                                    const ss = getStatus(gi);
                                    const isLeft = di === 0;
                                    const isRight = di === display.length - 1;
                                    let refKey = null;
                                    if (isLeft) refKey = `r${ri}-left`;
                                    if (isRight) refKey = `r${ri}-right`;
                                    if (isLeft && isRight) refKey = `r${ri}-left`;

                                    const showLine = di < display.length - 1;
                                    let lineColor = '#dee2e6';
                                    if (showLine) {
                                        const ngi = isLR ? gOff + di + 1 : gOff + (row.length - 1 - (di + 1));
                                        lineColor = isLR ? lnCol(gi, ngi) : lnCol(ngi, gi);
                                    }

                                    const isEdgeNode = (gi + 1) % NODES_PER_ROW === 0;
                                    const labelAbove = isEdgeNode;

                                    return (
                                        <div key={di} style={{ display: 'contents' }}>
                                            <div style={{ position: 'relative', flexShrink: 0 }}>
                                                {ri === 0 && !labelAbove && (
                                                    <div style={{ position: 'absolute', bottom: ss === 'current' ? 'calc(100% + 35px)' : 'calc(100% + 4px)', left: '50%', transform: 'translateX(-50%)', fontSize: '0.68rem', color: isAct(ss) ? '#6c757d' : '#ccc', whiteSpace: 'nowrap' }}>
                                                        {stage.date ? formatDate(stage.date) : '\u00A0'}
                                                    </div>
                                                )}
                                                {labelAbove && (
                                                    <>
                                                        <div style={{ position: 'absolute', bottom: ss === 'current' ? 'calc(100% + 37px)' : 'calc(100% + 20px)', left: '50%', transform: 'translateX(-50%)', fontSize: '0.68rem', color: isAct(ss) ? '#6c757d' : '#ccc', whiteSpace: 'nowrap' }}>
                                                            {stage.date ? formatDate(stage.date) : '\u00A0'}
                                                        </div>
                                                        <div style={{ position: 'absolute', bottom: 'calc(100% + 5px)', left: '50%', transform: 'translateX(-50%)', fontSize: '0.72rem', fontWeight: isAct(ss) ? 600 : 400, color: isAct(ss) ? '#333' : '#adb5bd', textAlign: 'center', lineHeight: 1.2, whiteSpace: 'nowrap' }}>
                                                            {stage.label}
                                                        </div>
                                                    </>
                                                )}
                                                {renderCircle(ss, refKey)}
                                                {!labelAbove && (
                                                    <div style={{ position: 'absolute', top: 'calc(100% + 5px)', left: '50%', transform: 'translateX(-50%)', fontSize: '0.72rem', fontWeight: isAct(ss) ? 600 : 400, color: isAct(ss) ? '#333' : '#adb5bd', textAlign: 'center', lineHeight: 1.2, width: ri === 0 ? 'auto' : 120, whiteSpace: ri === 0 ? 'nowrap' : 'normal' }}>
                                                        {stage.label}
                                                    </div>
                                                )}
                                                {ss === 'current' && status !== 'selesai' && (
                                                    <div style={{ position: 'absolute', bottom: 'calc(100% + 22px)', left: '50%', transform: 'translateX(-50%)', fontSize: '0.58rem', color: '#fd7e14', fontStyle: 'italic', whiteSpace: 'nowrap' }}>
                                                        Sedang menunggu persetujuan
                                                    </div>
                                                )}
                                            </div>
                                            {showLine && <div style={{ flex: 1, height: 3, backgroundColor: lineColor, borderRadius: 2, minWidth: 20 }}></div>}
                                        </div>
                                    );
                                })}
                            </div>
                        );
                    })}
                    {curvePaths.length > 0 && (
                        <svg style={{ position: 'absolute', top: 0, left: 0, width: '100%', height: '100%', pointerEvents: 'none', overflow: 'visible' }}>
                            {curvePaths.map((cp, i) => (
                                <path key={i} d={cp.d} stroke={cp.color} strokeWidth="3" fill="none" strokeLinecap="round" />
                            ))}
                        </svg>
                    )}
                </div>
                {getWaitingText() && (
                    <div style={{ textAlign: 'center', marginTop: 10, fontSize: '0.82rem', color: '#856404', fontWeight: 500 }}>
                        <i className="fas fa-clock me-1"></i>
                        {getWaitingText()}
                    </div>
                )}
            </div>
        </div>
    );
}

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

    const handleSubmit = async () => {
        if (!confirm('Yakin ingin mengajukan pengajuan ini?')) return;
        const res = await apiPost(`/PengajuanApi/${id}/submit`);
        if (res) { setMsg(res.message); loadData(); }
    };

    const handleStatusChange = async (status, confirmMsg) => {
        if (!confirm(confirmMsg)) return;
        const res = await apiPatch(`/PengajuanApi/${id}/status`, {
            status,
            userId: user.userId,
            roleId: user.roleId
        });
        if (res) { setMsg(res.message || 'Status berhasil diperbarui'); loadData(); }
    };

    const handleFinishReview = () => handleStatusChange('Reviewed', 'Tandai review selesai?');

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

    const canAdminEdit = isAdminUnit && statusLower === 'draft';
    const canReviewerEdit = isReviewer && statusLower === 'review';
    const canEdit = canAdminEdit || canReviewerEdit;

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
