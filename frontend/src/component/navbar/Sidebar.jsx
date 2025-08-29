import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "./Sidebar.css";

const Sidebar = ({ role, userName, onLogout }) => {
  const location = useLocation();
  const navigate = useNavigate();

  const menuItems = {
    admin: [
      { name: "Dashboard", path: "/admin-dashboard" },
      { name: "User Management", path: "/user-management" },
      { name: "Employee Management", path: "/employees" },      
      { name: "Benefits Management" , path: "/benefits-management"},
      { name: "Payroll Policies", path: "/payroll-policies" },
      { name: "Compliance Reports", path: "/compliance" },
      { name: "Notifications", path: "/notifications/admin-notifications" },
      { name: "Audit Trail", path: "/audit" },
    ],
    payrollprocessor: [
      { name: "Dashboard", path: "/processor-dashboard" },
      { name: "Payroll Processing", path: "/payroll-processing" },
      { name: "Payroll Verification", path: "/payroll-verification" },
      { name: "Benefits", path: "/benefits" },
      { name: "Audit Trail", path: "/audit" },
      { name: "Notifications", path: "/notifications" },
    ],
    employee: [
      { name: "Dashboard", path: "/employee-dashboard" },
      { name: "My Pay Stubs", path: "/paystubs" },
      { name: "Personal Info", path: "/personal-info" },
      { name: "Time Sheets", path: "/timesheets" },
      { name: "Leave Requests", path: "/leave-requests" },
      { name: "Notifications", path: "/notifications/view-notifications" },
    ],
    manager: [
      { name: "Dashboard", path: "/manager-dashboard" },
      { name: "Team Payroll", path: "/team-payroll" },
      { name: "Leave Approvals", path: "/leave-approvals" },
      { name: "Notifications", path: "/notifications" },
    ],
  };

  const items = menuItems[role?.toLowerCase()] || [{ name: "Dashboard", path: "/dashboard" }];

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

      <button className="logout-btn" onClick={onLogout}>
        Logout
      </button>
    </aside>
  );
};

export default Sidebar;
