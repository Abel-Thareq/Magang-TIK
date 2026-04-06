import { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { apiGet, apiPut } from '../../api/api';

export default function PengajuanEdit() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [pejabats, setPejabats] = useState([]);
    const [form, setForm] = useState({ nomorSuratText: '', tanggalPengajuan: '', tahunAnggaran: '', jenisPengajuan: 'Belanja Modal', noSuratRektor: '', jabatan: '', idPejabat: '' });
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        apiGet('/PengajuanApi/pejabats').then(data => data && setPejabats(data));
        apiGet(`/PengajuanApi/${id}`).then(data => {
            if (data?.pengajuan) {
                const p = data.pengajuan;
                setForm({
                    nomorSuratText: p.noSuratRektor || '',
                    tanggalPengajuan: p.tanggalPengajuan?.split('T')[0] || '',
                    tahunAnggaran: p.tahunAnggaran || '',
                    jenisPengajuan: p.jenisPengajuan || 'Belanja Modal',
                    noSuratRektor: p.noSuratRektor || '',
                    jabatan: p.jabatan || '',
                    idPejabat: p.idPejabat || '',
                });
            }
        });
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        await apiPut(`/PengajuanApi/${id}`, {
            noSuratRektor: form.noSuratRektor,
            tanggalPengajuan: form.tanggalPengajuan,
            tahunAnggaran: form.tahunAnggaran ? Number(form.tahunAnggaran) : null,
            jabatan: form.jabatan,
            idPejabat: form.idPejabat ? Number(form.idPejabat) : null,
            jenisPengajuan: form.jenisPengajuan,
        });
        setLoading(false);
        navigate(`/pengajuan/${id}`);
    };

    return (
        <div className="fade-in">
            <div className="mb-4">
                <Link to={`/pengajuan/${id}`} className="btn-outline-custom mb-3" style={{ fontSize: '0.8rem' }}>
                    <i className="fas fa-arrow-left"></i> Kembali ke Detail
                </Link>
            </div>

            <form onSubmit={handleSubmit}>
                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-file-signature"></i> Data Surat Pengajuan</div>
                    <div className="row g-3">
                        <div className="col-md-6">
                            <label className="form-label">Nomor Surat Pengajuan <span className="required-star">*</span></label>
                            <input className="form-control" required value={form.nomorSuratText} onChange={e => setForm({ ...form, nomorSuratText: e.target.value })} />
                        </div>
                        <div className="col-md-6">
                            <label className="form-label">Tanggal Pengajuan <span className="required-star">*</span></label>
                            <input type="date" className="form-control" required value={form.tanggalPengajuan} onChange={e => setForm({ ...form, tanggalPengajuan: e.target.value })} />
                        </div>
                        <div className="col-md-6">
                            <label className="form-label">Tahun Anggaran Belanja Modal</label>
                            <input type="number" className="form-control" value={form.tahunAnggaran} onChange={e => setForm({ ...form, tahunAnggaran: e.target.value })} />
                        </div>
                        <div className="col-md-6">
                            <label className="form-label">Jenis Pengajuan</label>
                            <select className="form-select" value={form.jenisPengajuan} onChange={e => setForm({ ...form, jenisPengajuan: e.target.value })}>
                                <option value="Belanja Modal">Belanja Modal</option>
                            </select>
                        </div>
                    </div>
                </div>

                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-envelope-open-text"></i> Data Surat Rektor</div>
                    <div className="row g-3">
                        <div className="col-md-6">
                            <label className="form-label">No. Surat Rektor</label>
                            <input className="form-control" value={form.noSuratRektor} onChange={e => setForm({ ...form, noSuratRektor: e.target.value })} />
                        </div>
                        <div className="col-md-6">
                            <label className="form-label">Tanggal Surat Rektor</label>
                            <input className="form-control" disabled placeholder="Otomatis terisi" />
                        </div>
                    </div>
                </div>

                <div className="form-section">
                    <div className="form-section-title"><i className="fas fa-user-tie"></i> Data Penandatangan</div>
                    <div className="row g-3">
                        <div className="col-md-6">
                            <label className="form-label">Jabatan Penandatangan</label>
                            <input className="form-control" value={form.jabatan} onChange={e => setForm({ ...form, jabatan: e.target.value })} />
                        </div>
                        <div className="col-md-6">
                            <label className="form-label">Nama Pejabat Penandatangan</label>
                            <select className="form-select" value={form.idPejabat} onChange={e => setForm({ ...form, idPejabat: e.target.value })}>
                                <option value="">-- Pilih Pejabat --</option>
                                {pejabats.map(p => <option key={p.idUser} value={p.idUser}>{p.nama}</option>)}
                            </select>
                        </div>
                    </div>
                </div>

                <div className="d-flex gap-3">
                    <button type="submit" className="btn-primary-custom" disabled={loading}>
                        <i className="fas fa-save"></i> {loading ? 'Menyimpan...' : 'Simpan Perubahan'}
                    </button>
                    <Link to={`/pengajuan/${id}`} className="btn-outline-custom">
                        <i className="fas fa-times"></i> Batal
                    </Link>
                </div>
            </form>
        </div>
    );
}
