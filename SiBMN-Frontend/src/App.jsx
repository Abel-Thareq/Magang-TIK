import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import Layout from './components/Layout';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import PengajuanIndex from './pages/pengajuan/PengajuanIndex';
import PengajuanCreate from './pages/pengajuan/PengajuanCreate';
import PengajuanEdit from './pages/pengajuan/PengajuanEdit';
import PengajuanDetails from './pages/pengajuan/PengajuanDetails';
import DetailPengajuanCreate from './pages/detailpengajuan/DetailPengajuanCreate';
import DetailPengajuanEdit from './pages/detailpengajuan/DetailPengajuanEdit';
import KodeBarangIndex from './pages/kodebarang/KodeBarangIndex';
import BarangModalIndex from './pages/barangmodal/BarangModalIndex';

function ProtectedRoute({ children }) {
  const { user, loading } = useAuth();
  if (loading) return <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '100vh' }}><i className="fas fa-spinner fa-spin fa-2x"></i></div>;
  if (!user) return <Navigate to="/login" />;
  return children;
}

function AppRoutes() {
  const { user } = useAuth();

  return (
    <Routes>
      <Route path="/login" element={user ? <Navigate to="/dashboard" /> : <Login />} />
      <Route path="/" element={<Navigate to={user ? "/dashboard" : "/login"} />} />

      <Route element={<ProtectedRoute><Layout /></ProtectedRoute>}>
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/pengajuan" element={<PengajuanIndex />} />
        <Route path="/pengajuan/create" element={<PengajuanCreate />} />
        <Route path="/pengajuan/edit/:id" element={<PengajuanEdit />} />
        <Route path="/pengajuan/:id" element={<PengajuanDetails />} />
        <Route path="/detailpengajuan/create/:pengajuanId" element={<DetailPengajuanCreate />} />
        <Route path="/detailpengajuan/edit/:id" element={<DetailPengajuanEdit />} />
        <Route path="/kodebarang" element={<KodeBarangIndex />} />
        <Route path="/barangmodal" element={<BarangModalIndex />} />
      </Route>
    </Routes>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}
