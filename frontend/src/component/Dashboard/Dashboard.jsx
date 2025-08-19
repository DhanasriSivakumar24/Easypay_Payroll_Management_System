import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./dashboard.css";

const Dashboard = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState({
    username: "",
    role: "",
  });

  useEffect(() => {
    const username = sessionStorage.getItem("username");
    const role = sessionStorage.getItem("role");
    if (!username || !role) {
      navigate("/login"); // redirect if not logged in
    } else {
      setUser({ username, role });
    }
  }, [navigate]);

  return (
    <div className="dashboard-container">
      <h1 className="dashboard-title">Welcome, {user.username} 👋</h1>
      <p className="dashboard-subtitle">
        {user.role === "ADMIN"
          ? "Here’s your Admin Dashboard"
          : "Here’s your Employee Dashboard"}
      </p>

      {/* Role-specific Data */}
      {user.role === "ADMIN" ? (
        <div className="admin-section">
          <h2>Admin Controls</h2>
          <ul>
            <li>📊 View All Employees</li>
            <li>📝 Manage Payroll</li>
            <li>✅ Approve/Reject Leaves</li>
            <li>👥 Manage Users</li>
          </ul>
        </div>
      ) : (
        <div className="employee-section">
          <h2>Employee Info</h2>
          <ul>
            <li>💰 View Salary Details</li>
            <li>📅 Check Leave Balance</li>
            <li>📜 Leave Request History</li>
            <li>👤 Update Profile</li>
          </ul>
        </div>
      )}
    </div>
  );
};

export default Dashboard;
