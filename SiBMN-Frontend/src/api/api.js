// ============================================
// MOCK API LAYER — No backend required
// All data is hardcoded for frontend-only demo
// ============================================

const delay = (ms = 300) => new Promise(r => setTimeout(r, ms));

// ========== MOCK DATA ==========

const MOCK_USERS = [
    { idUser: 1, nama: 'Abel Thareq', email: 'abel', roleName: 'Operator Unit Kerja', roleId: 1, unitId: 1, unitName: 'Fakultas Teknik', userId: 1 },
    { idUser: 2, nama: 'Budi Santoso', email: 'budi', roleName: 'Tim Kerja BMN', roleId: 4, unitId: null, unitName: null, userId: 2 },
    { idUser: 3, nama: 'Akmal Hasan', email: 'akmal', roleName: 'Tim Kerja BMN', roleId: 4, unitId: null, unitName: null, userId: 3 },
    { idUser: 4, nama: 'Dr. Siti Aminah', email: 'siti', roleName: 'Pimpinan Tim BMN', roleId: 5, unitId: null, unitName: null, userId: 4 },
    { idUser: 5, nama: 'Prof. Rahman', email: 'rahman', roleName: 'Pimpinan Unit Kerja', roleId: 6, unitId: 1, unitName: 'Fakultas Teknik', userId: 5 },
    { idUser: 6, nama: 'Dr. Wahyu', email: 'wahyu', roleName: 'WR BPKU', roleId: 7, unitId: null, unitName: null, userId: 6 },
    { idUser: 7, nama: 'Ir. Hendra', email: 'hendra', roleName: 'Kabiro BPKU', roleId: 8, unitId: null, unitName: null, userId: 7 },
    { idUser: 8, nama: 'Drs. Joko', email: 'joko', roleName: 'Kabag Umum', roleId: 9, unitId: null, unitName: null, userId: 8 },
];

const MOCK_PEJABATS = [
    { idUser: 5, nama: 'Prof. Rahman' },
    { idUser: 6, nama: 'Dr. Wahyu' },
];

let nextPengajuanId = 4;
let nextDetailId = 10;

let MOCK_PENGAJUANS = [
    {
        idPengajuan: 1, noSuratRektor: 'B/123/UN.01/KU/2026', tanggalPengajuan: '2026-03-15T00:00:00',
        tahunAnggaran: 2027, jabatan: 'Dekan Fakultas Teknik', idPejabat: 5, pejabatName: 'Prof. Rahman',
        jenisPengajuan: 'Belanja Modal', unitId: 1, unitName: 'Fakultas Teknik', status: 'Draft',
        totalHarga: 45000000, detailCount: 2, submittedAt: null, pimpinanUnitApprovedAt: null,
        wrBpkuApprovedAt: null, kabiroBpkuApprovedAt: null, reviewedAt: null, reviewedById: null,
        reviewedByName: null, approvedAt: null, approvedByName: null, kabagUmumApprovedAt: null,
    },
    {
        idPengajuan: 2, noSuratRektor: 'B/456/UN.01/KU/2026', tanggalPengajuan: '2026-03-20T00:00:00',
        tahunAnggaran: 2027, jabatan: 'Dekan Fakultas Teknik', idPejabat: 5, pejabatName: 'Prof. Rahman',
        jenisPengajuan: 'Belanja Modal', unitId: 1, unitName: 'Fakultas Teknik', status: 'Menunggu Tim BMN',
        totalHarga: 120000000, detailCount: 3, submittedAt: '2026-03-21T10:00:00',
        pimpinanUnitApprovedAt: '2026-03-22T09:00:00', wrBpkuApprovedAt: '2026-03-23T11:00:00',
        kabiroBpkuApprovedAt: '2026-03-24T14:00:00', reviewedAt: null, reviewedById: null,
        reviewedByName: null, approvedAt: null, approvedByName: null, kabagUmumApprovedAt: null,
    },
    {
        idPengajuan: 3, noSuratRektor: 'B/789/UN.01/KU/2026', tanggalPengajuan: '2026-02-10T00:00:00',
        tahunAnggaran: 2027, jabatan: 'Dekan Fakultas Teknik', idPejabat: 5, pejabatName: 'Prof. Rahman',
        jenisPengajuan: 'Belanja Modal', unitId: 1, unitName: 'Fakultas Teknik', status: 'Selesai',
        totalHarga: 85000000, detailCount: 2, submittedAt: '2026-02-11T08:00:00',
        pimpinanUnitApprovedAt: '2026-02-12T09:00:00', wrBpkuApprovedAt: '2026-02-13T10:00:00',
        kabiroBpkuApprovedAt: '2026-02-14T11:00:00', reviewedAt: '2026-02-15T14:00:00', reviewedById: 2,
        reviewedByName: 'Budi Santoso', approvedAt: '2026-02-16T09:00:00', approvedByName: 'Dr. Siti Aminah',
        kabagUmumApprovedAt: '2026-02-17T10:00:00',
    },
];

let MOCK_DETAILS = [
    { idDetPengajuan: 1, idPengajuan: 1, idBarang: 101, barangKode: '3.06.02.01.005', barangNama: 'Laptop', spesifikasi: 'Intel i7, RAM 16GB, SSD 512GB', jumlahDiminta: 5, hargaSatuan: 5000000, jumlahHarga: 25000000, idRuang: 1, gedungNama: 'Gedung A', ruangNama: 'Lab Komputer', fungsiBarang: 'Kegiatan praktikum', asalBarang: 'PDN', alasanImport: '', linkSurvey: '', linkGambar: '', noPrioritas: 1, isExcluded: false },
    { idDetPengajuan: 2, idPengajuan: 1, idBarang: 102, barangKode: '3.06.02.01.010', barangNama: 'Proyektor', spesifikasi: 'Full HD, 3500 Lumens', jumlahDiminta: 2, hargaSatuan: 10000000, jumlahHarga: 20000000, idRuang: 2, gedungNama: 'Gedung A', ruangNama: 'Ruang Kelas 101', fungsiBarang: 'Media pembelajaran', asalBarang: 'PDN', alasanImport: '', linkSurvey: '', linkGambar: '', noPrioritas: 2, isExcluded: false },
    { idDetPengajuan: 3, idPengajuan: 2, idBarang: 103, barangKode: '3.06.02.04.001', barangNama: 'Server Rack', spesifikasi: 'Xeon E5, 64GB RAM, 2TB SSD RAID', jumlahDiminta: 1, hargaSatuan: 80000000, jumlahHarga: 80000000, idRuang: 3, gedungNama: 'Gedung B', ruangNama: 'Server Room', fungsiBarang: 'Hosting aplikasi', asalBarang: 'Import', alasanImport: 'Spesifikasi tinggi tidak tersedia lokal', linkSurvey: '', linkGambar: '', noPrioritas: 1, isExcluded: false },
    { idDetPengajuan: 4, idPengajuan: 2, idBarang: 104, barangKode: '3.06.02.01.020', barangNama: 'Monitor', spesifikasi: '27 inch 4K IPS', jumlahDiminta: 10, hargaSatuan: 3000000, jumlahHarga: 30000000, idRuang: 1, gedungNama: 'Gedung A', ruangNama: 'Lab Komputer', fungsiBarang: 'Monitor workstation', asalBarang: 'PDN', alasanImport: '', linkSurvey: '', linkGambar: '', noPrioritas: 2, isExcluded: false },
    { idDetPengajuan: 5, idPengajuan: 2, idBarang: 105, barangKode: '3.06.02.01.015', barangNama: 'Printer Laser', spesifikasi: 'A3 Color Duplex', jumlahDiminta: 2, hargaSatuan: 5000000, jumlahHarga: 10000000, idRuang: 4, gedungNama: 'Gedung A', ruangNama: 'Ruang TU', fungsiBarang: 'Cetak dokumen', asalBarang: 'PDN', alasanImport: '', linkSurvey: '', linkGambar: '', noPrioritas: 3, isExcluded: false },
    { idDetPengajuan: 6, idPengajuan: 3, idBarang: 101, barangKode: '3.06.02.01.005', barangNama: 'Laptop', spesifikasi: 'Intel i5, RAM 8GB, SSD 256GB', jumlahDiminta: 10, hargaSatuan: 7000000, jumlahHarga: 70000000, idRuang: 1, gedungNama: 'Gedung A', ruangNama: 'Lab Komputer', fungsiBarang: 'Penggantian laptop lama', asalBarang: 'PDN', alasanImport: '', linkSurvey: '', linkGambar: '', noPrioritas: 1, isExcluded: false },
    { idDetPengajuan: 7, idPengajuan: 3, idBarang: 106, barangKode: '3.06.01.03.001', barangNama: 'AC Split', spesifikasi: '2 PK Inverter', jumlahDiminta: 3, hargaSatuan: 5000000, jumlahHarga: 15000000, idRuang: 2, gedungNama: 'Gedung A', ruangNama: 'Ruang Kelas 101', fungsiBarang: 'Pendingin ruangan', asalBarang: 'PDN', alasanImport: '', linkSurvey: '', linkGambar: '', noPrioritas: 2, isExcluded: false },
];

let MOCK_EVENTS = [
    { id: 1, userId: 1, bulan: '2026-04', waktu: '08:00', keterangan: 'Rapat koordinasi BMN' },
    { id: 2, userId: 1, bulan: '2026-04', waktu: '13:00', keterangan: 'Review pengajuan Q2' },
];
let nextEventId = 3;

const MOCK_KODE_BARANGS = [
    { id: 1, kodeBarangLengkap: '3', uraianBarang: 'Aset Tetap', level: 'Golongan' },
    { id: 2, kodeBarangLengkap: '3.06', uraianBarang: 'Aset Tetap Lainnya', level: 'Bidang' },
    { id: 3, kodeBarangLengkap: '3.06.02', uraianBarang: 'Peralatan dan Mesin', level: 'Kelompok' },
    { id: 4, kodeBarangLengkap: '3.06.02.01', uraianBarang: 'Alat Komputer', level: 'Sub Kelompok' },
    { id: 5, kodeBarangLengkap: '3.06.02.01.005', uraianBarang: 'Laptop', level: 'Kode Barang' },
    { id: 6, kodeBarangLengkap: '3.06.02.01.010', uraianBarang: 'Proyektor', level: 'Kode Barang' },
    { id: 7, kodeBarangLengkap: '3.06.02.01.015', uraianBarang: 'Printer Laser', level: 'Kode Barang' },
    { id: 8, kodeBarangLengkap: '3.06.02.01.020', uraianBarang: 'Monitor', level: 'Kode Barang' },
    { id: 9, kodeBarangLengkap: '3.06.02.04', uraianBarang: 'Alat Server', level: 'Sub Kelompok' },
    { id: 10, kodeBarangLengkap: '3.06.02.04.001', uraianBarang: 'Server Rack', level: 'Kode Barang' },
    { id: 11, kodeBarangLengkap: '3.06.01', uraianBarang: 'Gedung dan Bangunan', level: 'Kelompok' },
    { id: 12, kodeBarangLengkap: '3.06.01.03', uraianBarang: 'Alat Pendingin', level: 'Sub Kelompok' },
    { id: 13, kodeBarangLengkap: '3.06.01.03.001', uraianBarang: 'AC Split', level: 'Kode Barang' },
];

const MOCK_GOLONGANS = [{ kode: '3', uraian: 'Aset Tetap' }];
const MOCK_BIDANGS = [{ kode: '06', uraian: 'Aset Tetap Lainnya' }];
const MOCK_KELOMPOKS = [{ kode: '02', uraian: 'Peralatan dan Mesin' }, { kode: '01', uraian: 'Gedung dan Bangunan' }];
const MOCK_SUB_KELOMPOKS = [{ kode: '01', uraian: 'Alat Komputer' }, { kode: '04', uraian: 'Alat Server' }];

const MOCK_GEDUNGS = ['Gedung A', 'Gedung B', 'Gedung C', 'Gedung Rektorat'];
const MOCK_RUANGS = {
    'Gedung A': [
        { idRuang: 1, namaRuang: 'Lab Komputer' },
        { idRuang: 2, namaRuang: 'Ruang Kelas 101' },
        { idRuang: 4, namaRuang: 'Ruang TU' },
    ],
    'Gedung B': [
        { idRuang: 3, namaRuang: 'Server Room' },
        { idRuang: 5, namaRuang: 'Ruang Kelas 201' },
    ],
    'Gedung C': [
        { idRuang: 6, namaRuang: 'Laboratorium Fisika' },
    ],
    'Gedung Rektorat': [
        { idRuang: 7, namaRuang: 'Ruang Rapat' },
    ],
};

const MOCK_BARANG_LIST = [
    { id: 101, display: '3.06.02.01.005 - Laptop' },
    { id: 102, display: '3.06.02.01.010 - Proyektor' },
    { id: 103, display: '3.06.02.04.001 - Server Rack' },
    { id: 104, display: '3.06.02.01.020 - Monitor' },
    { id: 105, display: '3.06.02.01.015 - Printer Laser' },
    { id: 106, display: '3.06.01.03.001 - AC Split' },
];

// ========== MOCK API ROUTER ==========

function routeMock(url, method, data) {
    // --- AUTH ---
    if (url === '/api/AuthApi/login' && method === 'POST') {
        const user = MOCK_USERS.find(u => u.email === data.email);
        if (user) return { token: 'mock-jwt-token-' + user.idUser, user };
        throw new Error('Email atau password salah');
    }

    // --- DASHBOARD ---
    if (url.startsWith('/api/DashboardApi/stats')) {
        const params = new URLSearchParams(url.split('?')[1] || '');
        const unitId = Number(params.get('unitId'));
        const roleId = Number(params.get('roleId'));
        const relevant = roleId === 1 ? MOCK_PENGAJUANS.filter(p => p.unitId === unitId) : MOCK_PENGAJUANS;
        return {
            totalPengajuan: relevant.length,
            draftCount: relevant.filter(p => p.status === 'Draft').length,
            approvedCount: relevant.filter(p => p.status !== 'Draft').length,
            totalBarang: MOCK_DETAILS.length,
            totalAset: MOCK_KODE_BARANGS.filter(k => k.level === 'Kode Barang').length,
            asetPerGolongan: [
                { uraian: 'Alat Komputer', count: 4 },
                { uraian: 'Alat Server', count: 1 },
                { uraian: 'Alat Pendingin', count: 1 },
            ],
            recentPengajuan: relevant.slice(0, 5).map(p => ({
                idPengajuan: p.idPengajuan,
                tanggalPengajuan: p.tanggalPengajuan,
                unitName: p.unitName,
                jenisPengajuan: p.jenisPengajuan,
            })),
        };
    }

    // --- JADWAL (Calendar) ---
    if (url.startsWith('/api/JadwalApi') && method === 'GET') {
        const params = new URLSearchParams(url.split('?')[1] || '');
        const userId = Number(params.get('userId'));
        const bulan = params.get('bulan');
        return MOCK_EVENTS.filter(e => e.userId === userId && e.bulan === bulan);
    }
    if (url === '/api/JadwalApi' && method === 'POST') {
        const ev = { id: nextEventId++, ...data };
        MOCK_EVENTS.push(ev);
        return ev;
    }
    if (url.startsWith('/api/JadwalApi/') && method === 'DELETE') {
        const id = Number(url.split('/').pop());
        MOCK_EVENTS = MOCK_EVENTS.filter(e => e.id !== id);
        return { message: 'Deleted' };
    }

    // --- PENGAJUAN ---
    if (url.startsWith('/api/PengajuanApi/pejabats')) {
        return MOCK_PEJABATS;
    }
    if (url.match(/^\/api\/PengajuanApi\/\d+\/submit$/) && method === 'POST') {
        const id = Number(url.split('/')[3]);
        const p = MOCK_PENGAJUANS.find(x => x.idPengajuan === id);
        if (p) { p.status = 'Menunggu Pimpinan Unit'; p.submittedAt = new Date().toISOString(); }
        return { message: 'Pengajuan berhasil diajukan' };
    }
    if (url.match(/^\/api\/PengajuanApi\/\d+\/status$/) && method === 'PATCH') {
        const id = Number(url.split('/')[3]);
        const p = MOCK_PENGAJUANS.find(x => x.idPengajuan === id);
        if (p) {
            const statusActions = {
                'Review': () => { p.status = 'Review'; p.reviewedById = data.userId; p.reviewedByName = MOCK_USERS.find(u => u.userId === data.userId)?.nama; p.reviewedAt = new Date().toISOString(); },
                'Reviewed': () => { p.status = 'Reviewed'; },
                'Approve': () => { p.status = 'Menunggu Kabag Umum'; p.approvedAt = new Date().toISOString(); p.approvedByName = MOCK_USERS.find(u => u.userId === data.userId)?.nama; },
                'Reject': () => { p.status = 'Draft'; p.reviewedById = null; p.reviewedByName = null; },
                'ApprovePimpinanUnit': () => { p.status = 'Menunggu WR BPKU'; p.pimpinanUnitApprovedAt = new Date().toISOString(); },
                'RejectPimpinanUnit': () => { p.status = 'Draft'; },
                'ApproveWrBpku': () => { p.status = 'Menunggu Kabiro BPKU'; p.wrBpkuApprovedAt = new Date().toISOString(); },
                'RejectWrBpku': () => { p.status = 'Draft'; },
                'ApproveKabiroBpku': () => { p.status = 'Menunggu Tim BMN'; p.kabiroBpkuApprovedAt = new Date().toISOString(); },
                'RejectKabiroBpku': () => { p.status = 'Draft'; },
                'ApproveKabagUmum': () => { p.status = 'Selesai'; p.kabagUmumApprovedAt = new Date().toISOString(); },
                'RejectKabagUmum': () => { p.status = 'Draft'; },
            };
            const action = statusActions[data.status];
            if (action) action();
        }
        return { message: 'Status berhasil diperbarui' };
    }
    if (url.match(/^\/api\/PengajuanApi\/\d+(\?.*)?$/) && method === 'GET') {
        const id = Number(url.split('/')[3].split('?')[0]);
        const p = MOCK_PENGAJUANS.find(x => x.idPengajuan === id);
        const details = MOCK_DETAILS.filter(d => d.idPengajuan === id);
        return p ? { pengajuan: p, details } : null;
    }
    if (url.match(/^\/api\/PengajuanApi\/\d+(\?.*)?$/) && method === 'PUT') {
        const id = Number(url.split('/')[3].split('?')[0]);
        const p = MOCK_PENGAJUANS.find(x => x.idPengajuan === id);
        if (p) Object.assign(p, data);
        return { message: 'Updated' };
    }
    if (url.match(/^\/api\/PengajuanApi\/\d+(\?.*)?$/) && method === 'DELETE') {
        const id = Number(url.split('/')[3].split('?')[0]);
        MOCK_PENGAJUANS = MOCK_PENGAJUANS.filter(x => x.idPengajuan !== id);
        MOCK_DETAILS = MOCK_DETAILS.filter(d => d.idPengajuan !== id);
        return { message: 'Deleted' };
    }
    if (url.startsWith('/api/PengajuanApi') && method === 'GET') {
        const params = new URLSearchParams(url.split('?')[1] || '');
        const unitId = Number(params.get('unitId'));
        const roleId = Number(params.get('roleId'));
        if (roleId === 1) return MOCK_PENGAJUANS.filter(p => p.unitId === unitId);
        return MOCK_PENGAJUANS;
    }
    if (url === '/api/PengajuanApi' && method === 'POST') {
        const p = {
            idPengajuan: nextPengajuanId++, ...data, status: 'Draft', totalHarga: 0, detailCount: 0,
            unitName: 'Fakultas Teknik', pejabatName: MOCK_PEJABATS.find(x => x.idUser === data.idPejabat)?.nama || '',
            submittedAt: null, pimpinanUnitApprovedAt: null, wrBpkuApprovedAt: null, kabiroBpkuApprovedAt: null,
            reviewedAt: null, reviewedById: null, reviewedByName: null, approvedAt: null, approvedByName: null, kabagUmumApprovedAt: null,
        };
        MOCK_PENGAJUANS.push(p);
        return { id: p.idPengajuan };
    }

    // --- DETAIL PENGAJUAN ---
    if (url.startsWith('/api/DetailPengajuanApi/dropdowns')) {
        return { gedungs: MOCK_GEDUNGS };
    }
    if (url.startsWith('/api/DetailPengajuanApi/kodebarangs')) {
        const params = new URLSearchParams(url.split('?')[1] || '');
        const search = (params.get('search') || '').toLowerCase();
        let result = [...MOCK_BARANG_LIST];
        if (search) result = result.filter(b => b.display.toLowerCase().includes(search));
        return result;
    }
    if (url.match(/^\/api\/DetailPengajuanApi\/\d+\/toggle-exclude$/) && method === 'PATCH') {
        const id = Number(url.split('/')[3]);
        const d = MOCK_DETAILS.find(x => x.idDetPengajuan === id);
        if (d) d.isExcluded = !d.isExcluded;
        return { message: 'Toggled' };
    }
    if (url.match(/^\/api\/DetailPengajuanApi\/\d+\/moveup$/) && method === 'POST') {
        const id = Number(url.split('/')[3]);
        const d = MOCK_DETAILS.find(x => x.idDetPengajuan === id);
        if (d && d.noPrioritas > 1) {
            const siblings = MOCK_DETAILS.filter(x => x.idPengajuan === d.idPengajuan);
            const swap = siblings.find(x => x.noPrioritas === d.noPrioritas - 1);
            if (swap) { swap.noPrioritas++; d.noPrioritas--; }
        }
        return { message: 'Moved' };
    }
    if (url.match(/^\/api\/DetailPengajuanApi\/\d+\/movedown$/) && method === 'POST') {
        const id = Number(url.split('/')[3]);
        const d = MOCK_DETAILS.find(x => x.idDetPengajuan === id);
        if (d) {
            const siblings = MOCK_DETAILS.filter(x => x.idPengajuan === d.idPengajuan);
            const swap = siblings.find(x => x.noPrioritas === d.noPrioritas + 1);
            if (swap) { swap.noPrioritas--; d.noPrioritas++; }
        }
        return { message: 'Moved' };
    }
    if (url.match(/^\/api\/DetailPengajuanApi\/\d+(\?.*)?$/) && method === 'GET') {
        const id = Number(url.split('/')[3].split('?')[0]);
        return MOCK_DETAILS.find(x => x.idDetPengajuan === id) || null;
    }
    if (url.match(/^\/api\/DetailPengajuanApi\/\d+(\?.*)?$/) && method === 'PUT') {
        const id = Number(url.split('/')[3]);
        const d = MOCK_DETAILS.find(x => x.idDetPengajuan === id);
        if (d) {
            Object.assign(d, data);
            d.jumlahHarga = (d.jumlahDiminta || 0) * (d.hargaSatuan || 0);
            const barang = MOCK_BARANG_LIST.find(b => b.id === d.idBarang);
            if (barang) {
                d.barangNama = barang.display.split(' - ')[1];
                d.barangKode = barang.display.split(' - ')[0];
            }
            recalcTotal(d.idPengajuan);
        }
        return { message: 'Updated' };
    }
    if (url.match(/^\/api\/DetailPengajuanApi\/\d+(\?.*)?$/) && method === 'DELETE') {
        const id = Number(url.split('/')[3]);
        const d = MOCK_DETAILS.find(x => x.idDetPengajuan === id);
        MOCK_DETAILS = MOCK_DETAILS.filter(x => x.idDetPengajuan !== id);
        if (d) recalcTotal(d.idPengajuan);
        return { message: 'Deleted' };
    }
    if (url === '/api/DetailPengajuanApi' && method === 'POST') {
        const barang = MOCK_BARANG_LIST.find(b => b.id === data.idBarang);
        const siblings = MOCK_DETAILS.filter(x => x.idPengajuan === data.idPengajuan);
        const detail = {
            idDetPengajuan: nextDetailId++, ...data,
            jumlahHarga: (data.jumlahDiminta || 0) * (data.hargaSatuan || 0),
            barangNama: barang ? barang.display.split(' - ')[1] : 'Unknown',
            barangKode: barang ? barang.display.split(' - ')[0] : '0.00.00.00.000',
            gedungNama: data.gedung || '', ruangNama: 'Ruang',
            noPrioritas: siblings.length + 1, isExcluded: false,
        };
        for (const [gedung, rooms] of Object.entries(MOCK_RUANGS)) {
            const room = rooms.find(r => r.idRuang === data.idRuang);
            if (room) { detail.gedungNama = gedung; detail.ruangNama = room.namaRuang; break; }
        }
        MOCK_DETAILS.push(detail);
        recalcTotal(data.idPengajuan);
        return { id: detail.idDetPengajuan };
    }

    // --- BARANG (ruangs) ---
    if (url.startsWith('/api/BarangApi/GetRuangs')) {
        const params = new URLSearchParams(url.split('?')[1] || '');
        const gedung = params.get('gedung');
        return MOCK_RUANGS[gedung] || [];
    }

    // --- KODE BARANG ---
    if (url.startsWith('/api/KodeBarangApi/Golongan')) return MOCK_GOLONGANS;
    if (url.startsWith('/api/KodeBarangApi/Bidang')) return MOCK_BIDANGS;
    if (url.startsWith('/api/KodeBarangApi/Kelompok')) return MOCK_KELOMPOKS;
    if (url.startsWith('/api/KodeBarangApi/SubKelompok')) return MOCK_SUB_KELOMPOKS;
    if (url.match(/^\/api\/KodeBarangApi\/\d+$/) && method === 'PUT') {
        const id = Number(url.split('/').pop());
        const item = MOCK_KODE_BARANGS.find(k => k.id === id);
        if (item) item.uraianBarang = data.uraianBarang;
        return { message: 'Updated' };
    }
    if (url.match(/^\/api\/KodeBarangApi\/\d+$/) && method === 'DELETE') {
        const id = Number(url.split('/').pop());
        const idx = MOCK_KODE_BARANGS.findIndex(k => k.id === id);
        if (idx >= 0) MOCK_KODE_BARANGS.splice(idx, 1);
        return { message: 'Deleted' };
    }
    if (url.startsWith('/api/KodeBarangApi') && method === 'GET') {
        const params = new URLSearchParams(url.split('?')[1] || '');
        const filterLevel = params.get('filterLevel');
        let result = [...MOCK_KODE_BARANGS];
        if (filterLevel) result = result.filter(k => k.level === filterLevel || k.level === filterLevel.replace('SubKelompok', 'Sub Kelompok').replace('KodeBarang', 'Kode Barang'));
        return result;
    }
    if (url === '/api/KodeBarangApi' && method === 'POST') {
        const maxId = Math.max(...MOCK_KODE_BARANGS.map(k => k.id), 0);
        const item = { id: maxId + 1, kodeBarangLengkap: data.kodeGolongan || '', uraianBarang: data.uraianBarang, level: data.level };
        MOCK_KODE_BARANGS.push(item);
        return { id: item.id, message: 'Created' };
    }

    console.warn('[MOCK API] Unhandled:', method, url);
    return {};
}

function recalcTotal(pengajuanId) {
    const p = MOCK_PENGAJUANS.find(x => x.idPengajuan === pengajuanId);
    if (p) {
        const items = MOCK_DETAILS.filter(d => d.idPengajuan === pengajuanId && !d.isExcluded);
        p.totalHarga = items.reduce((sum, d) => sum + (d.jumlahHarga || 0), 0);
        p.detailCount = MOCK_DETAILS.filter(d => d.idPengajuan === pengajuanId).length;
    }
}

// ========== PUBLIC API (same interface as original) ==========

export async function apiGet(url) {
    await delay();
    return routeMock(`/api${url}`, 'GET', null);
}

export async function apiPost(url, data) {
    await delay();
    return routeMock(`/api${url}`, 'POST', data);
}

export async function apiPut(url, data) {
    await delay();
    return routeMock(`/api${url}`, 'PUT', data);
}

export async function apiDelete(url) {
    await delay();
    return routeMock(`/api${url}`, 'DELETE', null);
}

export async function apiPatch(url, data) {
    await delay();
    return routeMock(`/api${url}`, 'PATCH', data);
}

export function formatRupiah(angka) {
    const num = Number(angka) || 0;
    return 'Rp ' + num.toLocaleString('id-ID');
}

export function formatDate(dateStr, format = 'short') {
    if (!dateStr) return '-';
    const d = new Date(dateStr);
    if (format === 'long') {
        return d.toLocaleDateString('id-ID', { day: '2-digit', month: 'long', year: 'numeric' });
    }
    return d.toLocaleDateString('id-ID', { day: '2-digit', month: 'short', year: 'numeric' });
}
