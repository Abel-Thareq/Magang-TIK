import { Outlet, NavLink, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useState } from 'react';

export default function Layout() {
    const { user, logout } = useAuth();
    const location = useLocation();
    const [sidebarOpen, setSidebarOpen] = useState(false);

    const handleLogout = () => {
        logout();
        window.location.href = '/login';
    };

    return (
        <>
            {/* Sidebar */}
            <aside className={`sidebar ${sidebarOpen ? 'show' : ''}`}>
                <div className="brand">
                    <img src="/logo-untidar.png" alt="Logo" className="brand-logo" />
                    <span>SiBMN</span>
                </div>
                <div className="sidebar-divider"></div>
                <ul className="menu-list">
                    <li>
                        <NavLink to="/dashboard" className={({ isActive }) => `menu-item ${isActive ? 'active' : ''}`} onClick={() => setSidebarOpen(false)}>
                            <i className="fas fa-home"></i> Beranda
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/pengajuan" end className={({ isActive }) => `menu-item ${isActive ? 'active' : ''}`} onClick={() => setSidebarOpen(false)}>
                            <i className="fas fa-list-alt"></i> Daftar Pengajuan
                        </NavLink>
                    </li>
                    {user?.roleId === 1 && (
                        <li>
                            <NavLink to="/pengajuan/create" className={({ isActive }) => `menu-item ${isActive ? 'active' : ''}`} onClick={() => setSidebarOpen(false)}>
                                <i className="fas fa-file-medical"></i> Buat Pengajuan
                            </NavLink>
                        </li>
                    )}
                    {user?.roleId === 4 && (
                        <li>
                            <NavLink to="/kodebarang" className={({ isActive }) => `menu-item ${isActive ? 'active' : ''}`} onClick={() => setSidebarOpen(false)}>
                                <i className="fas fa-database"></i> Data Master
                            </NavLink>
                        </li>
                    )}
                </ul>
                <div className="sidebar-divider"></div>
                <div style={{ flex: 1 }}></div>
                <div className="logout-btn-container">
                    <button className="logout-btn" onClick={handleLogout}>
                        <i className="fas fa-sign-out-alt"></i> Logout
                    </button>
                </div>
            </aside>

            {/* Main Content */}
            <main className="main-wrapper">
                <header className="topbar">
                    <button className="sidebar-toggle d-lg-none" onClick={() => setSidebarOpen(true)}>
                        <i className="fas fa-bars"></i>
                    </button>
                    <div className="search-bar">
                        <i className="fas fa-search"></i>
                        <input type="text" placeholder="Cari disini ..." />
                    </div>
                    <div className="topbar-right">
                        <div className="icon-btn"><i className="fas fa-cog"></i></div>
                        <div className="icon-btn">
                            <i className="far fa-bell"></i>
                        </div>
                        <div className="user-profile">
                            <div className="user-info">
                                <div className="user-name">{user?.nama}</div>
                                <div className="user-role">{user?.roleName}</div>
                            </div>
                            <div className="avatar">
                                <i className="fas fa-user"></i>
                            </div>
                        </div>
                    </div>
                </header>

                <div className="page-content">
                    <Outlet />
                </div>
            </main>

            {/* Mobile overlay */}
            {sidebarOpen && <div className="sidebar-overlay" onClick={() => setSidebarOpen(false)} />}
        </>
    );
}
