const API_BASE = '/api';

function getToken() {
    return sessionStorage.getItem('token');
}

async function apiFetch(url, options = {}) {
    const token = getToken();
    const headers = {
        'Content-Type': 'application/json',
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...options.headers,
    };

    const response = await fetch(`${API_BASE}${url}`, { ...options, headers });

    if (response.status === 401) {
        sessionStorage.clear();
        window.location.href = '/login';
        return null;
    }

    return response;
}

export async function apiGet(url) {
    const res = await apiFetch(url);
    if (!res) return null;
    return res.json();
}

export async function apiPost(url, data) {
    const res = await apiFetch(url, {
        method: 'POST',
        body: JSON.stringify(data),
    });
    if (!res) return null;
    return res.json();
}

export async function apiPut(url, data) {
    const res = await apiFetch(url, {
        method: 'PUT',
        body: JSON.stringify(data),
    });
    if (!res) return null;
    return res.json();
}

export async function apiDelete(url) {
    const res = await apiFetch(url, { method: 'DELETE' });
    if (!res) return null;
    return res.json();
}

export async function apiPatch(url, data) {
    const res = await apiFetch(url, {
        method: 'PATCH',
        body: JSON.stringify(data),
    });
    if (!res) return null;
    return res.json();
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
