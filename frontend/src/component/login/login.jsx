import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { login } from "../../authSlicer";  // ✅ import redux action
import { loginAPICall } from "../../service/login.service";
import { LoginModel } from "../../models/login.model";
import { LoginErrorModel } from "../../models/loginerror.model";
import "./login.css";

const Login = () => {
  const [user, setUser] = useState(new LoginModel());
  const [error, setError] = useState(new LoginErrorModel());
  const navigate = useNavigate();
  const dispatch = useDispatch(); // redux dispatcher

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser((u) => ({ ...u, [name]: value }));
    setError((err) => ({
      ...err,
      [name]: value ? "" : `${name.charAt(0).toUpperCase() + name.slice(1)} is required`,
    }));
  };

  const handleLogin = () => {
    if (error.username || error.password) return;

    loginAPICall(user)
      .then((res) => {
        const data = res.data;

        dispatch(
          login({
            username: data.userName,
            role: data.role,
            token: data.token,
            employeeId: data.employeeId,
          })
        );

        // Optional
        sessionStorage.setItem("token", data.token);
        sessionStorage.setItem("username", data.userName);
        sessionStorage.setItem("employeeId", data.employeeId);
        sessionStorage.setItem("role", data.role);

        const role = data.role?.toUpperCase();

        if (role === "ADMIN") {
  navigate("/admin-dashboard");
} else if (role === "EMPLOYEE") {
  navigate("/employee-dashboard");
} else {
  navigate("/dashboard");
}
      })
      .catch((err) => alert(err.response?.data?.error || "Login failed"));
  };

  const handleCancel = () => {
    setUser(new LoginModel());
    setError(new LoginErrorModel());
  };

  return (
    <div className="login-container">
      {/* Left Section (Form) */}
      <div className="login-form-section">
        <div className="login-box">
          <div className="login-logo">Payroll System</div>

          <h1 className="login-title">Welcome Back 👋</h1>
          <p className="login-subtitle">Get into your account to check Updates</p>

          <input
            type="text"
            name="username"
            placeholder="Username"
            value={user.username}
            onChange={handleChange}
            className="login-input"
          />
          {error.username && <div className="login-error">{error.username}</div>}

          <input
            type="password"
            name="password"
            placeholder="Password"
            value={user.password}
            onChange={handleChange}
            className="login-input"
          />
          {error.password && <div className="login-error">{error.password}</div>}

          <div className="login-actions">
            <button className="login-button" onClick={handleLogin}>
              Login
            </button>
            <button className="cancel-button" onClick={handleCancel}>
              Cancel
            </button>
          </div>
        </div>
      </div>

      {/* Right Section (Blue Background with Illustration) */}
      <div className="login-illustration-section">
        <img
          src="/login.png"
          alt="Payroll Illustration"
          className="login-illustration"
        />
      </div>
    </div>
  );
};

export default Login;
