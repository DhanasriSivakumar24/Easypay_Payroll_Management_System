import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import './Sidebar.css';

const Sidebar = ({ role, userName, onLogout }) => {
  const location = useLocation();
  const navigate = useNavigate();

  const menuItems = {
    admin: [
      { name: "Dashboard", path: "/dashboard" },
      { name: "Employees", path: "/employees" },
      { name: "Leave Requests", path: "/leaves" },
      { name: "Payroll", path: "/payroll" },
      { name: "Policies", path: "/policies" },
      { name: "Audit Trail", path: "/audit" },
      { name: "Notifications", path: "/notifications" },
    ],
  };

  const items = menuItems[role.toLowerCase()] || [{ name: "Dashboard", path: "/dashboard" }];

  return (
    <aside className="sidebar">
      <div className="sidebar-header">
        <h2>Easypay</h2>
        <p>Welcome, {userName}</p>
      </div>

      <ul className="sidebar-menu">
        {items.map((item) => (
          <li
            key={item.name}
            className={location.pathname.startsWith(item.path) ? "active" : ""}
            onClick={() => navigate(item.path)}
          >
            {item.name}
          </li>
        ))}
      </ul>

      <button className="logout-btn" onClick={onLogout}>Logout</button>
    </aside>
  );
};

export default Sidebar;
