import { createContext, useContext, useState, useEffect } from 'react';

const AuthContext = createContext(null);

// Mock user accounts for frontend-only demo
const MOCK_USERS = [
    { userId: 1, idUser: 1, nama: 'Abel Thareq', email: 'abel', roleName: 'Operator Unit Kerja', roleId: 1, unitId: 1, unitName: 'Fakultas Teknik' },
    { userId: 2, idUser: 2, nama: 'Budi Santoso', email: 'budi', roleName: 'Tim Kerja BMN', roleId: 4, unitId: null, unitName: null },
    { userId: 3, idUser: 3, nama: 'Akmal Hasan', email: 'akmal', roleName: 'Tim Kerja BMN', roleId: 4, unitId: null, unitName: null },
    { userId: 4, idUser: 4, nama: 'Dr. Siti Aminah', email: 'siti', roleName: 'Pimpinan Tim BMN', roleId: 5, unitId: null, unitName: null },
    { userId: 5, idUser: 5, nama: 'Prof. Rahman', email: 'rahman', roleName: 'Pimpinan Unit Kerja', roleId: 6, unitId: 1, unitName: 'Fakultas Teknik' },
    { userId: 6, idUser: 6, nama: 'Dr. Wahyu', email: 'wahyu', roleName: 'WR BPKU', roleId: 7, unitId: null, unitName: null },
    { userId: 7, idUser: 7, nama: 'Ir. Hendra', email: 'hendra', roleName: 'Kabiro BPKU', roleId: 8, unitId: null, unitName: null },
    { userId: 8, idUser: 8, nama: 'Drs. Joko', email: 'joko', roleName: 'Kabag Umum', roleId: 9, unitId: null, unitName: null },
];

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = sessionStorage.getItem('token');
        const savedUser = sessionStorage.getItem('user');
        if (token && savedUser) {
            setUser(JSON.parse(savedUser));
        }
        setLoading(false);
    }, []);

    const login = async (email, password) => {
        // Mock login — any password works, match by email/username
        await new Promise(r => setTimeout(r, 500)); // simulate network delay

        const foundUser = MOCK_USERS.find(u => u.email === email);
        if (!foundUser) {
            throw new Error('Username tidak ditemukan. Coba: abel, budi, akmal, siti, rahman, wahyu, hendra, joko');
        }

        sessionStorage.setItem('token', 'mock-jwt-token-' + foundUser.userId);
        sessionStorage.setItem('user', JSON.stringify(foundUser));
        setUser(foundUser);
        return foundUser;
    };

    const logout = () => {
        sessionStorage.clear();
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    return useContext(AuthContext);
}
