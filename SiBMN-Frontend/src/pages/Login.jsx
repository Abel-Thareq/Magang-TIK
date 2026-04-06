import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [showPw, setShowPw] = useState(false);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);
        try {
            await login(email, password);
            navigate('/dashboard');
        } catch (err) {
            setError(err.message || 'Login gagal');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="login-page">
            <div className="login-bg"></div>
            <div className="login-container">
                {/* LEFT: Logos + Welcome Text */}
                <div className="login-left">
                    <div className="login-logos">
                        <img src="/logo-untidar.png" alt="Logo Untidar" className="login-logo-img" />
                        <img src="/logo-BLU.png" alt="Logo BLU" className="login-logo-img" />
                    </div>
                    <div className="login-welcome">
                        <p className="welcome-text">Welcome Back</p>
                        <p className="welcome-text welcome-to">To</p>
                        <h1 className="welcome-brand">SiBMN</h1>
                    </div>
                </div>

                {/* DIVIDER */}
                <div className="login-divider"></div>

                {/* RIGHT: Form */}
                <div className="login-right">
                    <form onSubmit={handleSubmit}>
                        {error && <div className="login-error">{error}</div>}

                        <div className="login-input-wrap">
                            <i className="fas fa-user login-field-icon"></i>
                            <input
                                type="text"
                                className="login-field"
                                placeholder="Username"
                                autoComplete="username"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                            />
                        </div>

                        <div className="login-input-wrap">
                            <i className="fas fa-lock login-field-icon"></i>
                            <input
                                type={showPw ? 'text' : 'password'}
                                className="login-field"
                                placeholder="Password"
                                autoComplete="current-password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                            <button type="button" className="login-eye-btn" onClick={() => setShowPw(!showPw)}>
                                <i className={`fas ${showPw ? 'fa-eye-slash' : 'fa-eye'}`}></i>
                            </button>
                        </div>

                        <div className="login-btn-wrap-right">
                            <button type="submit" className="login-btn-submit" disabled={loading}>
                                {loading ? 'Loading...' : 'Login'}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
