import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { apiGet, formatDate } from '../api/api';

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
    const [currentDate, setCurrentDate] = useState(new Date());
    const [events, setEvents] = useState(() => {
        const saved = sessionStorage.getItem('sibmn-events');
        return saved ? JSON.parse(saved) : [];
    });
    const [showAddEvent, setShowAddEvent] = useState(false);
    const [newEvent, setNewEvent] = useState({ time: '', description: '' });

    const monthNames = ['Januari', 'Februari', 'Maret', 'April', 'Mei', 'Juni', 'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'];
    const dayLabels = ['S', 'S', 'R', 'K', 'J', 'S', 'M'];

    // Get the Monday of the current week
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
    const monthEvents = events.filter(e => e.month === monthKey);

    const addEvent = () => {
        if (!newEvent.time || !newEvent.description) return;
        const updated = [...events, { ...newEvent, month: monthKey, id: Date.now() }];
        setEvents(updated);
        sessionStorage.setItem('sibmn-events', JSON.stringify(updated));
        setNewEvent({ time: '', description: '' });
        setShowAddEvent(false);
    };

    const deleteEvent = (id) => {
        const updated = events.filter(e => e.id !== id);
        setEvents(updated);
        sessionStorage.setItem('sibmn-events', JSON.stringify(updated));
    };

    return (
        <div className="dash-calendar">
            <div className="dash-cal-top">
                <button className="dash-cal-add-btn" onClick={() => setShowAddEvent(!showAddEvent)} title="Tambah Jadwal">
                    <i className="fas fa-plus"></i>
                </button>
                <div className="dash-cal-month-label">
                    <i className="fas fa-calendar-alt"></i>
                    <span>{monthNames[displayMonth.getMonth()]} <i className="fas fa-caret-down" style={{ fontSize: '0.7rem', marginLeft: '2px' }}></i></span>
                </div>
            </div>

            <div className="dash-cal-week-header">
                {dayLabels.map((d, i) => <span key={i} className="dash-cal-day-label">{d}</span>)}
            </div>

            <div className="dash-cal-week-row">
                <button className="dash-cal-arrow" onClick={prevWeek}><i className="fas fa-chevron-left"></i></button>
                {weekDays.map((d, i) => (
                    <span key={i} className={`dash-cal-date ${isToday(d) ? 'today' : ''} ${isSunday(d) ? 'sunday' : ''}`}>
                        {d.getDate()}
                    </span>
                ))}
                <button className="dash-cal-arrow" onClick={nextWeek}><i className="fas fa-chevron-right"></i></button>
            </div>

            {/* Add Event Form */}
            {showAddEvent && (
                <div className="dash-cal-add-form">
                    <input type="time" value={newEvent.time} onChange={e => setNewEvent({ ...newEvent, time: e.target.value })} />
                    <input type="text" placeholder="Keterangan jadwal..." value={newEvent.description} onChange={e => setNewEvent({ ...newEvent, description: e.target.value })} />
                    <button onClick={addEvent}><i className="fas fa-check"></i></button>
                </div>
            )}

            {/* Events List */}
            {monthEvents.length > 0 && (
                <div className="dash-cal-events">
                    {monthEvents.map(e => (
                        <div key={e.id} className="dash-cal-event">
                            <span className="dash-cal-event-time">{e.time}</span>
                            <span className="dash-cal-event-text">{e.description}</span>
                            <button className="dash-cal-event-del" onClick={() => deleteEvent(e.id)}><i className="fas fa-times"></i></button>
                        </div>
                    ))}
                </div>
            )}
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
        const diffMs = now - d;
        const diffH = Math.floor(diffMs / (1000 * 60 * 60));
        if (diffH < 1) return 'Baru saja';
        if (diffH < 24) return `${diffH} jam lalu`;
        const diffD = Math.floor(diffH / 24);
        if (diffD === 1) return 'Kemarin';
        if (diffD < 7) return `${diffD} hari lalu`;
        return formatDate(dateStr);
    };

    return (
        <div className="fade-in">
            <div className="dash-layout">
                {/* LEFT COLUMN */}
                <div className="dash-left">
                    {/* Welcome Banner */}
                    <div className="dash-welcome-banner">
                        <div className="dash-welcome-text">
                            <h2>Hallo, {user?.nama?.split(' ')[0]}!!</h2>
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
                                        <span className={`dash-recent-badge ${item.status}`}>{item.status === 'draft' ? 'Draft' : 'Diajukan'}</span>
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
                        <div className="dash-stat-icon" style={{ background: 'linear-gradient(135deg, #3498DB, #5DADE2)' }}>
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
