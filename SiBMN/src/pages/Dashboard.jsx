import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { apiGet, apiPost, apiDelete, formatDate } from '../api/api';

const DONUT_COLORS = ['#4FC3F7', '#AB47BC', '#FF9800', '#EF5350', '#66BB6A', '#26C6DA', '#FFA726'];

function DonutChart({ data, total }) {
    const size = 180;
    const cx = size / 2;
    const cy = size / 2;
    const radius = 62;
    const strokeWidth = 28;

    let cumulative = 0;
    const segments = data.map((item, i) => {
        const pct = total > 0 ? item.count / total : 0;
        const dashArray = 2 * Math.PI * radius;
        const dashOffset = dashArray * (1 - pct);
        const rotation = cumulative * 360 - 90;
        cumulative += pct;

        return (
            <circle key={i} cx={cx} cy={cy} r={radius} fill="none"
                stroke={DONUT_COLORS[i % DONUT_COLORS.length]}
                strokeWidth={strokeWidth}
                strokeDasharray={`${dashArray}`}
                strokeDashoffset={`${dashOffset}`}
                transform={`rotate(${rotation} ${cx} ${cy})`}
                style={{ transition: 'stroke-dashoffset 0.8s ease' }}
            />
        );
    });

    return (
        <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`}>
            <circle cx={cx} cy={cy} r={radius} fill="none" stroke="#f0f0f0" strokeWidth={strokeWidth} />
            {segments}
            <text x={cx} y={cy - 6} textAnchor="middle" fontSize="10" fill="#999" fontWeight="500">Total Aset</text>
            <text x={cx} y={cy + 14} textAnchor="middle" fontSize="22" fill="#333" fontWeight="700">{total}</text>
        </svg>
    );
}

function WeekCalendar() {
    const { user } = useAuth();
    const [currentDate, setCurrentDate] = useState(new Date());
    const [events, setEvents] = useState([]);
    const [showAddEvent, setShowAddEvent] = useState(false);
    const [newEvent, setNewEvent] = useState({ time: '', description: '' });

    const monthNames = ['Januari', 'Februari', 'Maret', 'April', 'Mei', 'Juni', 'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'];
    const dayLabels = ['S', 'S', 'R', 'K', 'J', 'S', 'M'];

    const getWeekStart = (date) => {
        const d = new Date(date);
        const day = d.getDay();
        const diff = d.getDate() - day + (day === 0 ? -6 : 1);
        return new Date(d.setDate(diff));
    };

    const weekStart = getWeekStart(currentDate);
    const weekDays = [];
    for (let i = 0; i < 7; i++) {
        const d = new Date(weekStart);
        d.setDate(weekStart.getDate() + i);
        weekDays.push(d);
    }

    const today = new Date();
    const isToday = (d) => d.getDate() === today.getDate() && d.getMonth() === today.getMonth() && d.getFullYear() === today.getFullYear();
    const isSunday = (d) => d.getDay() === 0;

    const prevWeek = () => {
        const d = new Date(currentDate);
        d.setDate(d.getDate() - 7);
        setCurrentDate(d);
    };

    const nextWeek = () => {
        const d = new Date(currentDate);
        d.setDate(d.getDate() + 7);
        setCurrentDate(d);
    };

    const displayMonth = weekDays[3]; // Use middle of week for month name
    const monthKey = `${displayMonth.getFullYear()}-${String(displayMonth.getMonth() + 1).padStart(2, '0')}`;

    const loadEvents = () => {
        if (!user?.userId) return;
        apiGet(`/JadwalApi?userId=${user.userId}&bulan=${monthKey}`)
            .then(data => { if (data) setEvents(data); });
    };

    useEffect(() => {
        loadEvents();
    }, [user, monthKey]);

    const addEvent = async () => {
        try {
            if (!newEvent.time) return alert("Pilih waktu terlebih dahulu!");
            if (!newEvent.description) return alert("Isi keterangan jadwal!");
            if (!user) return alert("Error: User belum login / tidak terdeteksi");
            if (!user.userId && !user.idUser) return alert("Error: ID User tidak ditemukan dalam data sesi");

            const reqUserId = user.userId || user.idUser;

            const res = await apiPost(`/JadwalApi`, {
                userId: reqUserId,
                bulan: monthKey,
                waktu: newEvent.time,
                keterangan: newEvent.description
            });

            if (res) {
                setNewEvent({ time: '', description: '' });
                setShowAddEvent(false);
                loadEvents();
                alert("Jadwal berhasil ditambahkan!");
            } else {
                alert("Gagal menyimpan jadwal (API mengembalikan null)");
            }
        } catch (err) {
            console.error(err);
            alert("Terjadi kesalahan jaringan/server: " + err.message);
        }
    };

    const deleteEvent = async (id) => {
        const res = await apiDelete(`/JadwalApi/${id}`);
        if (res) loadEvents();
    };

    return (
        <div className="calendar-widget">
            <div className="cal-header-section">
                <div className="cal-top">
                    <i className="fas fa-plus-circle" style={{ fontSize: '18px', cursor: 'pointer' }} onClick={() => setShowAddEvent(!showAddEvent)}></i>
                    <div style={{ display: 'flex', alignItems: 'center', gap: '6px', fontWeight: 600 }}>
                        <i className="far fa-calendar-alt"></i>
                        <span>{monthNames[displayMonth.getMonth()]} <i className="fas fa-caret-down" style={{ fontSize: '0.7rem' }}></i></span>
                    </div>
                </div>
                <div className="cal-days">
                    {dayLabels.map((d, i) => <div key={i}>{d}</div>)}
                </div>
                <div className="cal-numbers">
                    <div><i className="fas fa-chevron-left" style={{ fontSize: '10px', cursor: 'pointer' }} onClick={prevWeek}></i></div>
                    {weekDays.map((d, i) => (
                        <div key={i} className={isSunday(d) ? 'sunday-date' : ''}>
                            {isToday(d) ? (
                                <div className="active-date">{d.getDate()}</div>
                            ) : d.getDate()}
                        </div>
                    ))}
                    <div><i className="fas fa-chevron-right" style={{ fontSize: '10px', cursor: 'pointer' }} onClick={nextWeek}></i></div>
                </div>
            </div>

            {/* Add Event Form */}
            {showAddEvent && (
                <div className="cal-add-form">
                    <input type="time" value={newEvent.time} onChange={e => setNewEvent({ ...newEvent, time: e.target.value })} />
                    <input type="text" placeholder="Keterangan jadwal..." value={newEvent.description} onChange={e => setNewEvent({ ...newEvent, description: e.target.value })} />
                    <button onClick={addEvent}><i className="fas fa-check"></i></button>
                </div>
            )}

            <div className="cal-events">
                {events.map(e => (
                    <div key={e.id} className="event-item">
                        <div className="event-time">{e.waktu}</div>
                        <div className="event-desc">{e.keterangan}</div>
                        <button className="event-del" onClick={() => deleteEvent(e.id)}><i className="fas fa-times"></i></button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default function Dashboard() {
    const { user } = useAuth();
    const [stats, setStats] = useState({
        totalPengajuan: 0, draftCount: 0, approvedCount: 0, totalBarang: 0,
        totalAset: 0, asetPerGolongan: [], recentPengajuan: []
    });

    useEffect(() => {
        apiGet(`/DashboardApi/stats?unitId=${user?.unitId}&roleId=${user?.roleId}`)
            .then(data => data && setStats(data));
    }, [user]);

    const getTimeDiff = (dateStr) => {
        if (!dateStr) return '';
        const d = new Date(dateStr);
        const now = new Date();
        const isSameDay = d.getDate() === now.getDate() && d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
        const yesterday = new Date(now);
        yesterday.setDate(yesterday.getDate() - 1);
        const isYesterday = d.getDate() === yesterday.getDate() && d.getMonth() === yesterday.getMonth() && d.getFullYear() === yesterday.getFullYear();

        if (isSameDay) return 'Hari Ini';
        if (isYesterday) return 'Kemarin';
        const hours = String(d.getHours()).padStart(2, '0');
        const minutes = String(d.getMinutes()).padStart(2, '0');
        return `${hours}.${minutes}`;
    };

    return (
        <div className="fade-in">
            <div className="dash-layout">
                {/* LEFT COLUMN */}
                <div className="dash-left">
                    {/* Welcome Banner */}
                    <div className="dash-welcome-banner">
                        <div className="dash-welcome-text">
                            <h2>Halo, {user?.nama?.split(' ')[0]}!!</h2>
                            <p>Kelola pengajuan barang modal unit kerja disini</p>
                            <Link to="/pengajuan/create" className="dash-welcome-btn">
                                <i className="fas fa-plus-circle"></i> Buat Pengajuan
                            </Link>
                        </div>
                        <img src="/dashboard-illustration.png" alt="Ilustrasi" className="dash-welcome-img" />
                    </div>

                    {/* Middle Row: Total Aset + Pengajuan Terbaru */}
                    <div className="dash-middle-row">
                        <div className="dash-card">
                            <h3 className="dash-card-title">Total Aset</h3>
                            <div className="dash-aset-content">
                                <DonutChart data={stats.asetPerGolongan} total={stats.totalAset} />
                                <div className="dash-aset-legend">
                                    {stats.asetPerGolongan.map((item, i) => (
                                        <div key={i} className="dash-legend-item">
                                            <span className="dash-legend-dot" style={{ background: DONUT_COLORS[i % DONUT_COLORS.length] }}></span>
                                            <span>{item.uraian}</span>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>

                        <div className="dash-card">
                            <h3 className="dash-card-title">Pengajuan Terbaru</h3>
                            <div className="dash-recent-list">
                                {stats.recentPengajuan.length > 0 ? stats.recentPengajuan.map(item => (
                                    <Link to={`/pengajuan/${item.idPengajuan}`} key={item.idPengajuan} className="dash-recent-item">
                                        <span className="dash-recent-time">{getTimeDiff(item.tanggalPengajuan)}</span>
                                        <span className="dash-recent-unit">{item.unitName}</span>
                                        <span className="dash-recent-type">{item.jenisPengajuan || 'Belanja Modal'}</span>
                                        <span className="dash-recent-badge lihat-detail">Lihat Detail</span>
                                    </Link>
                                )) : (
                                    <div className="text-center text-muted py-4" style={{ fontSize: '0.85rem' }}>Belum ada pengajuan</div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>

                {/* RIGHT COLUMN */}
                <div className="dash-right">
                    <div className="dash-stat-card">
                        <div className="dash-stat-icon" style={{ background: 'linear-gradient(135deg, #E74C3C, #FF6B6B)' }}>
                            <i className="fas fa-file-invoice"></i>
                        </div>
                        <div>
                            <div className="dash-stat-value">{stats.totalPengajuan}</div>
                            <div className="dash-stat-label">Total Pengajuan</div>
                        </div>
                    </div>

                    <div className="dash-stat-card">
                        <div className="dash-stat-icon" style={{ background: 'linear-gradient(135deg, #F39C12, #F1C40F)' }}>
                            <i className="fas fa-edit"></i>
                        </div>
                        <div>
                            <div className="dash-stat-value">{stats.draftCount}</div>
                            <div className="dash-stat-label">Total Draf</div>
                        </div>
                    </div>

                    <div className="dash-stat-card">
                        <div className="dash-stat-icon" style={{ background: 'linear-gradient(135deg, #F57F17, #FFB300)' }}>
                            <i className="fas fa-clipboard-check"></i>
                        </div>
                        <div>
                            <div className="dash-stat-value">{stats.approvedCount}</div>
                            <div className="dash-stat-label">Total Diajukan</div>
                        </div>
                    </div>

                    <WeekCalendar />
                </div>
            </div>
        </div>
    );
}
